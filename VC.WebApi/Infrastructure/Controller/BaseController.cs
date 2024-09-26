using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace VC.WebApi.Shared.Controller
{
    [ApiController]
    [Route("/api/v1/")]
    public abstract class BaseController : ControllerBase
    {
        protected ILogger log;
        public BaseController()
        {
            log = Serilog.Log.Logger.ForContext("SourceContext", GetType().FullName);
        }

    }
}
