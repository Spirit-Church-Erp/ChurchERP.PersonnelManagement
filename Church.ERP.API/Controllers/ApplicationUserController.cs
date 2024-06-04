using Church.ERP.Application.Implementation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Church.ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplicationUser([System.Web.Http.FromBody] CreateApplicationUser.Request command)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _mediator.Send(command));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
