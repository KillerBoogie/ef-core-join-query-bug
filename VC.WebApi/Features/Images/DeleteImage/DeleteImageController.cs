using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VC.WebApi.Infrastructure.Errors;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Controller;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.DeleteImage
{
    public class DeleteImageController(IMediator mediator) : BaseController
    {
        [Route("images/{id}")]
        [HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Tags = new[] { "Images" }, Summary = "Delete a image from the image collection.")]

        public async Task<ActionResult> DeleteImage(
            Guid id,
            [FromHeader(Name = "If-None-Match")] string? eTags,
            [FromQuery] EnvironmentDTO environmentDTO)
        {
            DeleteImageRequest request = new(id);
            var query = new DeleteImageCommand(request, environmentDTO);

            Result result = await mediator.Send(query);

            if (result.IsFailure)
            {
                ProblemDetail problemDetail = result.Errors.ToProblemDetail();
                return StatusCode(problemDetail.Status, problemDetail);
            }
            else
            {
                return Ok();
            }
        }
    };
}