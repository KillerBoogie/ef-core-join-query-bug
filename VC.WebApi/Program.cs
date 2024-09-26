using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using VC.WebApi.Features.Images.AddImage;
using VC.WebApi.Features.Images.DeleteImage;
using VC.WebApi.Features.Images.GetImage;
using VC.WebApi.Features.Images.GetImages;
using VC.WebApi.Features.Locations.AddLocation;
using VC.WebApi.Features.Locations.GetLocations;
using VC.WebApi.Infrastructure.Controller;
using VC.WebApi.Infrastructure.EFCore.Context;
using VC.WebApi.Infrastructure.JsonConverters;
using VC.WebApi.Infrastructure.MediatR.Pipelines;
using VC.WebApi.Infrastructure.MediatR.Piplines;
using VC.WebApi.Infrastructure.Middleware.Attributes;
using VC.WebApi.Infrastructure.Middleware.Exceptions;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Swagger;
using VC.WebApi.Shared.Texts;

var serilogConfiguration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(path: "serilog.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"serilog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(serilogConfiguration)
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();

Log.Information("Starting bootstrap...");
Log.Information("ASPNETCORE_ENVIRONMENT=\"" + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") + "\"");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configurationLog) =>
    {
        configurationLog
        .ReadFrom.Configuration(serilogConfiguration)
        .Enrich.FromLogContext()
        .ReadFrom.Services(services);
    });

    Log.Information("Bootstrap JsonOptions for Controllers...");
    //used for mediatR pipeline logging
    builder.Services.AddSingleton(provider =>
    {
        var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        options.Converters.Add(new ResultJsonConverterFactory());
        options.Converters.Add(new TextJsonConverterFactory());
        options.Converters.Add(new MLJsonConverterFactory());
        return options;
    });


    Log.Information("Bootstrap AddControllers...");
    builder.Services.AddControllers(
        options =>
        {
            options.Filters.Add<ETagFilter>(); // Add the ETag filter globally
            options.Filters.Add<SetLocationHeaderFilter>(); // Register the filter globally
            options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            options.JsonSerializerOptions.Converters.Add(new IpAddressJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new OptionalConverter());
            options.JsonSerializerOptions.Converters.Add(new EmptyStringConverter());
            options.AllowInputFormatterExceptionMessages = false;
        });

    Log.Information("Bootstrap JsonOptions for DI...");
    var jsonSerializerOptions = new JsonSerializerOptions
    {
        Converters = { new ResultJsonConverterFactory() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    //Add Repositories
    Log.Information("Bootstrap repository dependencies...");

    builder.Services.AddScoped<IAddLocationRepository, AddLocationRepository>();
    builder.Services.AddScoped<IGetLocationsRepository, GetLocationsRepository>();

    builder.Services.AddScoped<IAddImageRepository, AddImageRepository>();
    builder.Services.AddScoped<IGetImagesRepository, GetImagesRepository>();
    builder.Services.AddScoped<IGetImageRepository, GetImageRepository>();
    builder.Services.AddScoped<IDeleteImageRepository, DeleteImageRepository>();

    //Add DBContext
    Log.Information("Bootstrap DBContext...");
    builder.Services.AddDbContext<VCDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
        .ConfigureWarnings(e => e.Log(
            (RelationalEventId.CommandExecuted, Microsoft.Extensions.Logging.LogLevel.Trace)
            ))
        .LogTo(message => Debug.WriteLine(message)));

    // Add MediatR
    Log.Information("Bootstrap MediatR dependencies...");

    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });

    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    Log.Information("Bootstrap Swagger...");

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vintage Club Website", Version = "v1" });
        c.SchemaFilter<SwaggerIgnoreFilter>();
        c.OperationFilter<OpenApiIgnoreFilter>();
        c.OperationFilter<SwaggerResponseTypeOperationFilter>();
        c.EnableAnnotations();
    });

    //Model Binder
    Log.Information("Bootstrap ModelBinderProviders...");
    builder.Services.AddMvc(o =>
    {
        o.ModelBinderProviders.Insert(0, new ModelBinderProvider());
        o.Conventions.Add(new CommaSeparatedQueryStringConvention());
    });

    Log.Information("Bootstrap completed!");

    //Builder 
    Log.Information("Build app...");
    var app = builder.Build();

    Log.Information("Build app completed!");
    Log.Information("App.Environment: " + app.Environment.EnvironmentName);


    Log.Information("Add SerilogRequestLogging...");
    app.UseSerilogRequestLogging(
        options =>
        {
            options.MessageTemplate =
                "{Actor} {RemoteIpAddress} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            options.EnrichDiagnosticContext = (
                diagnosticContext,
                httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
                diagnosticContext.Set("Actor", httpContext.Items["Actor"]);
            };
        });

    // Configure the HTTP request pipeline.
    app.UseExceptionMiddleware();

    app.UseSwagger();
    app.UseSwaggerUI();

    Log.Information("Activate UseStaticFiles...");
    app.UseStaticFiles();

    Log.Information("Controller mapping initializing...");
    app.MapControllers();

    //Optionally recreate and init Database

    Log.Information("Ensuring Database...");
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<VCDbContext>();
        context.EnsureCreatedWithCustomScripts();
    }

    Log.Information("App ready to run...");
    app.Run();

    Log.CloseAndFlush();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly because of unhandled exception.");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

// needed for Test WebApplicationFactory
public partial class Program { }
