using Church.ERP.Application.Implementation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        [Route("CreateApplicationUser")]
        public async Task<IActionResult> CreateApplicationUser([FromBody] CreateApplicationUser.Request command)
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

        [HttpPost]
        [Route("CreateApplicationAdminUser")]
        public async Task<IActionResult> CreateApplicationAdminUser(CreateUserAsAdmin.Request command)
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

        [HttpPost("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles(GetAllRoles.Request command)
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
