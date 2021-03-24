using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLS.Service.Accounts.Commands;
using MLS.Service.Common;

namespace MLS.Api.Controllers
{
    [Route("v1/Account")]
    [ApiController]
    public class AccountController : BaseController
    {
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<OperationResult>> LoginAsync([Required] [FromBody] LoginCommand command,
            CancellationToken cancellationToken = default) => Ok(await Mediator.Send(command, cancellationToken));
        
        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<OperationResult>> RegisterAsync(
            [Required] [FromBody] CreateAccountCommand command, CancellationToken cancellationToken = default) 
            => Ok(await Mediator.Send(command, cancellationToken));
        
        [HttpPost("Logout")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<OperationResult>> LogoutAsync(LogoutCommand command,
            CancellationToken cancellationToken = default) => Ok(await Mediator.Send(command, cancellationToken));
    }
}