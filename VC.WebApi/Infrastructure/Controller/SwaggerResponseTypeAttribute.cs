namespace VC.WebApi.Infrastructure.Controller
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SwaggerResponseTypeAttribute : Attribute
    {
        public Type ResponseType { get; }

        public SwaggerResponseTypeAttribute(Type responseType)
        {
            ResponseType = responseType;
        }
    }
}
