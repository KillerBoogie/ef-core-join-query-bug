using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace VC.WebApi.Infrastructure.Middleware.ModelBinder
{
    public class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //if (context.Metadata.ModelType == typeof(RequestAttributes)) // we want to use our custom model binder when action parameter is of type RequestAttributes
            //{
            //    return new BinderTypeModelBinder(typeof(RequestAttributeModelBinder));
            //}

            //if (context.Metadata.Name == "preferredLanguages")   // && (context.Metadata.ModelType == typeof(List<Language>))
            //{
            //    return new BinderTypeModelBinder(typeof(AcceptLanguageBinder));
            //}

            if (context.Metadata.Name == "environmentDTO")   // && (context.Metadata.ModelType == typeof(List<Language>))
            {
                return new BinderTypeModelBinder(typeof(EnvironmentDTOBinder));
            }
            //if ((context.Metadata.Name == "preferredLanguages"))   // && (context.Metadata.ModelType == typeof(List<Language>))
            //{
            //    return new BinderTypeModelBinder(typeof(LanguagesModelBinder));
            //}

            else
            {
                return null; // ignore all other types
            }
        }
    }
}
