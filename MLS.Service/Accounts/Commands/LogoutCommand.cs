using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using MLS.Service.Common;

namespace MLS.Service.Accounts.Commands
{
    public class LogoutCommand : IRequest<OperationResult>
    {
    }
}
