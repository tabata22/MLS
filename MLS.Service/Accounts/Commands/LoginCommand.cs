using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using MLS.Service.Common;

namespace MLS.Service.Accounts.Commands
{
    public class LoginCommand : IRequest<OperationResult<string>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
