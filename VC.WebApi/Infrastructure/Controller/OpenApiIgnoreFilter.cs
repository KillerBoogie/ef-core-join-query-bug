namespace VC.WebApi.Infrastructure.Controller
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class OpenApiIgnoreAttribute : Attribute
    {
    }

    public class OpenApiIgnoreFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        public void Apply(Microsoft.OpenApi.Models.OpenApiOperation operation, Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
        {
            if (operation == null || context == null || context.ApiDescription?.ParameterDescriptions == null)
                return;

            var parametersToHide = context.ApiDescription.ParameterDescriptions
                .Where(parameterDescription => ParameterHasIgnoreAttribute(parameterDescription))
                .ToList();

            if (parametersToHide.Count == 0)
                return;

            foreach (var parameterToHide in parametersToHide)
            {
                var parameter = operation.Parameters.FirstOrDefault(parameter => string.Equals(parameter.Name, parameterToHide.Name, StringComparison.Ordinal));
                if (parameter != null)
                    operation.Parameters.Remove(parameter);
            }
        }

        private static bool ParameterHasIgnoreAttribute(Microsoft.AspNetCore.Mvc.ApiExplorer.ApiParameterDescription parameterDescription)
        {
            if (parameterDescription.ModelMetadata is Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultModelMetadata metadata)
            {
                bool result = metadata.Attributes.Attributes?.Any(attribute => attribute.GetType() == typeof(OpenApiIgnoreAttribute)) ?? false;
                return result;
            }

            return false;
        }
    }
}
