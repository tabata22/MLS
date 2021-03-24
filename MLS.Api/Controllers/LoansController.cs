using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MLS.Service.Common;
using MLS.Service.Loans;
using MLS.Service.Loans.Commands;

namespace MLS.Api.Controllers
{
    [Route("v1/Loans")]
    [ApiController]
    public class LoansController : BaseController
    {
        [HttpPost("Save")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<OperationResult>> SaveAsync([Required] [FromBody] CreateLoanCommand command,
            CancellationToken cancellationToken = default) => Ok(await Mediator.Send(command, cancellationToken));
       
        [HttpPost("Update")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<OperationResult>> UpdateAsync([Required] [FromBody] UpdateLoanCommand command,
            CancellationToken cancellationToken = default) => Ok(await Mediator.Send(command, cancellationToken));
        
        [HttpGet("Get")]
        [ProducesResponseType(typeof(IEnumerable<LoanViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<LoanViewModel>>> GetAsync([FromQuery] GetLoanCommand command,
            CancellationToken cancellationToken = default) => Ok(await Mediator.Send(command, cancellationToken));
    }
}