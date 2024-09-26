using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VC.WebApi.Infrastructure.Errors;
using VC.WebApi.Infrastructure.Middleware.Attributes;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Controller;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.GetImages
{
    public class GetImagesController(IMediator mediator) : BaseController
    {
        [Route("images")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ImageMLResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ImageSLResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Tags = ["Images"], Summary = "Get all images multi-language or in preferred language with fall backs")]

        public async Task<IActionResult> GetAllImages(
            [FromHeader(Name = "If-None-Match")] string? eTags,
            [FromQuery][CommaSeparated] IEnumerable<string> preferredlLanguages,
            [FromQuery] EnvironmentDTO environmentDTO)
        {
            List<Language> langs = Language.ConformLanguageRanges(preferredlLanguages);

            GetImagesRequest request = new(langs);

            var query = new GetImagesQuery(request, environmentDTO);
            log.Information("GetImagesQuery" + request.ToString());

            Result<ImagesResponse> result = await mediator.Send(query);

            if (result.IsFailure)
            {
                ProblemDetail problemDetail = result.Errors.ToProblemDetail();
                return StatusCode(problemDetail.Status, problemDetail);
            }
            ImagesResponse response = result.Value;

            if (response.HasValue())
            {
                return response.SingleLanguageResponse is not null ?
                    Ok(response.SingleLanguageResponse) :
                    Ok(response.MultiLanguageResponse);
            }
            else
            {
                return new JsonResult("[]");
            }
        }
    };
}