using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MLS.Service.Loans
{
    public enum LoanTypeEnum
    {
        [Description("სწრაფი სესხი")]
        FastLoan = 1,
        [Description("ავტო სესხი")]
        AutoLoan = 2,
        [Description("განვადება")]
        Installment = 3
    }
}
