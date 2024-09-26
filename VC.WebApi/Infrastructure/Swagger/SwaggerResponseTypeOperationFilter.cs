using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using VC.WebApi.Infrastructure.Controller;

namespace VC.WebApi.Shared.Swagger
{
    public class SwaggerResponseTypeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodAttributes = context.MethodInfo.GetCustomAttributes(true);

            foreach (var attribute in methodAttributes)
            {
                if (attribute is SwaggerResponseTypeAttribute responseTypeAttribute)
                {
                    var schema = context.SchemaGenerator.GenerateSchema(responseTypeAttribute.ResponseType, context.SchemaRepository);
                    var mediaType = new OpenApiMediaType { Schema = schema };

                    operation.Responses["200"] = new OpenApiResponse
                    {
                        Description = "Success",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = mediaType
                        }
                    };
                }
            }
        }
    }
}
