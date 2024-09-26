using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VC.WebApi.Infrastructure.Controller
{
    public class SetLocationHeaderFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // This method can be left empty if you don't need to do anything before the action executes.
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.StatusCode == 201)
            {
                var routeData = context.RouteData;
                var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

                if (controllerActionDescriptor != null)
                {
                    // Generate the location URL using the route information
                    var controllerName = controllerActionDescriptor.ControllerName;
                    var actionName = controllerActionDescriptor.ActionName; // Use the action name from the descriptor

                    var routeAttribute = controllerActionDescriptor.MethodInfo
                    .GetCustomAttributes(typeof(RouteAttribute), false)
                    .Cast<RouteAttribute>()
                    .FirstOrDefault() ??
                    controllerActionDescriptor.ControllerTypeInfo
                    .GetCustomAttributes(typeof(RouteAttribute), false)
                    .Cast<RouteAttribute>()
                    .FirstOrDefault();

                    if (routeAttribute != null)
                    {
                        var routeTemplate = routeAttribute.Template;

                        // Assuming the id is part of the route data or result value
                        var id = routeData.Values["id"] ?? (context.Result as ObjectResult)?.Value?.GetType().GetProperty("Id")?.GetValue((context.Result as ObjectResult)?.Value, null);

                        if (id != null)
                        {
                            var actionContext = new ActionContext(context.HttpContext, routeData, context.ActionDescriptor);

                            // Construct the location URL using the base path and id
                            var locationUrl = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}/{id}";

                            // Set the Location header
                            context.HttpContext.Response.Headers.Location = locationUrl;
                        }
                    }
                }
            }
        }
    }
}


