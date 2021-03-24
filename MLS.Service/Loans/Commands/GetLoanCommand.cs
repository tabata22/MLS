using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using MLS.Service.Common;

namespace MLS.Service.Loans.Commands
{
    public class GetLoanCommand : IRequest<OperationResult<IEnumerable<LoanViewModel>>>
    {

    }
}
