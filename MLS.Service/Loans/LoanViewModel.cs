using System;
using System.Collections.Generic;
using System.Text;

namespace MLS.Service.Loans
{
    public class LoanViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int LoanTypeId { get; set; }
        public int LoanStatusId { get; set; }
        public decimal Balance { get; set; }
        public string Ccy { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
