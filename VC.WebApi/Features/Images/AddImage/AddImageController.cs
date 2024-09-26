using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VC.WebApi.Infrastructure.Controller;
using VC.WebApi.Infrastructure.Errors;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Controller;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Responses;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.AddImage
{
    public class AddImageController(IMediator mediator) : BaseController
    {
        [Route("images")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponseType(typeof(GuidIdResponse))]
        [SwaggerOperation(Tags = new[] { "Images" }, Summary = "Add an (exernal) image to the image collection.")]

        public async Task<ActionResult<GuidIdResponse>> AddImage(AddImageRequest request,
            [FromHeader(Name = "If-None-Match")] string? eTags,
            [FromQuery] EnvironmentDTO environmentDTO)
        {
            var query = new AddImageCommand(request, environmentDTO);

            Result<GuidIdResponse> result = await mediator.Send(query);

            if (result.IsFailure)
            {
                ProblemDetail problemDetail = result.Errors.ToProblemDetail();
                return StatusCode(problemDetail.Status, problemDetail);
            }

            return StatusCode(StatusCodes.Status201Created, result.Value);
        }
    }
}