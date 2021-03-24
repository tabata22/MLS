using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MediatR;
using MLS.Service.Common;

namespace MLS.Service.Loans.Commands
{
    public class CreateLoanCommand : IRequest<OperationResult>
    {
        public LoanTypeEnum LoanTypeId { get; set; }
        public decimal Balance { get; set; }
        public string Ccy { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
