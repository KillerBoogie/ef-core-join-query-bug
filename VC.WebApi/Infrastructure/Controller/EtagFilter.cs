
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace VC.WebApi.Infrastructure.Controller
{
    public class ETagFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {

            string responseBody;

            if (context.Result is ObjectResult objectResult)
            {
                // Serialize the response object to a JSON string
                responseBody = JsonSerializer.Serialize(objectResult.Value);
                GenerateAndSetETag(context, responseBody, objectResult.StatusCode);
            }
            else if (context.Result is JsonResult jsonResult)
            {
                responseBody = (string)(jsonResult.Value ?? "{}");
                GenerateAndSetETag(context, responseBody, jsonResult.StatusCode ?? 200);
            }

            base.OnActionExecuted(context);
        }

        static void GenerateAndSetETag(ActionExecutedContext context, string responseBody, int? statusCode)
        {
            // Generate the ETag
            using (var sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(responseBody));
                string etag = Convert.ToBase64String(hash).TrimEnd('=');

                // Set the ETag header
                context.HttpContext.Response.Headers["ETag"] = $"\"{etag}\"";

                // Set the response body to the serialized JSON string
                context.Result = new ContentResult
                {
                    Content = responseBody,
                    ContentType = "application/json",
                    StatusCode = statusCode
                };
            }
        }
    }
}