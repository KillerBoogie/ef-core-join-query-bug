using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace VC.WebApi.Infrastructure.EFCore.Context
{
    public static class DbContextExtensions
    {
        public static void EnsureCreatedWithCustomScripts(this VCDbContext context)
        {
            var deleted = context.Database.EnsureDeleted();
            var created = context.Database.EnsureCreated();
            if (created)
            {
                // Execute custom scripts
                var scriptFilePaths = new List<string>
                {
                    "Infrastructure/EFCore/Scripts/CreateGetLanguageUDF.sql",
                    "Infrastructure/EFCore/Scripts/CreateGetExactLanguageUDF.sql",
                    "Infrastructure/EFCore/Scripts/CreateGetLanguageJsonUDF.sql",
                    "Infrastructure/EFCore/Scripts/CreateGetExactLanguageJsonUDF.sql",
                    "Infrastructure/EFCore/Scripts/InitCountryData.sql",

                    "Infrastructure/EFCore/Scripts/InsertLocationData.sql",
                };

                foreach (var scriptFilePath in scriptFilePaths)
                {
                    //read file and escape json braces because internally it uses String.Format
                    var script = File.ReadAllText(scriptFilePath).Replace("{", "{{").Replace("}", "}}");
                    Log.Information("Running SQL-Script: " + scriptFilePath);
                    context.Database.ExecuteSqlRaw(script);
                }
            }

        }

        public static string getInsertSql(List<string> languages, Type type, Type enumType)
        {
            string typeName = type.Name;

            List<(string key, Dictionary<string, string> translations)> enumTranslations = GetTranslations(languages, type, enumType);

            StringBuilder sql = new();

            sql.Append($"INSERT VC.{typeName}([{typeName}Id], [Name], [Created], [CreatedBy], [CreatedInNameOf], [LastModified], [LastModifiedBy], [LastModifiedInNameOf]) Values");

            StringBuilder values = new();

            foreach ((string Key, Dictionary<string, string> Translations) item in enumTranslations)
            {
                if (values.Length > 0) { values.Append(", "); }

                values.Append($"('{item.Key}', N'[");

                StringBuilder name = new StringBuilder();

                foreach (KeyValuePair<string, string> i in item.Translations)
                {
                    string language = i.Key;
                    string translation = i.Value;

                    if (name.Length > 0) { name.Append(", "); }

                    name.Append($"{{\"language\": \"{language}\", \"value\":\"{translation}\"}}");
                }
                values.Append(name);
                values.Append("]', getDate(), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', getDate(), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')");
            }

            sql.Append(values);

            return sql.ToString();
        }
        public static List<(string key, Dictionary<string, string> translations)> GetTranslations(List<string> languages, Type type, Type enumType)
        {
            List<string> enums = new(Enum.GetNames(enumType));
            List<(string key, Dictionary<string, string> translations)> enumTranslations = new();

            ResourceManager resourceManager = new(type.FullName!, Assembly.GetExecutingAssembly());

            foreach (string key in enums)
            {
                Dictionary<string, string> translations = [];
                translations = new();

                foreach (var lang in languages)
                {
                    CultureInfo culture = new(lang);

                    string? translation = resourceManager.GetString(key, culture);

                    if (translation is null)
                    {
                        throw new NullReferenceException($"In resource '{type}' key '{key}' has no value for culture '{culture}' .");
                    }
                    translations[lang] = translation;
                }

                enumTranslations.Add((key, translations));

            }

            return enumTranslations;
        }
    }
}
