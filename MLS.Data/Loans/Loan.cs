using System;
using MLS.Data.LoanStatuses;
using MLS.Data.LoanTypes;

namespace MLS.Data.Loans
{
    public class Loan
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int LoanTypeId { get; set; }
        public LoanType LoanType { get; set; }
        public int LoanStatusId { get; set; }
        public LoanStatus LoanStatus { get; set; }
        public decimal Balance { get; set; }
        public string Ccy { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
