using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MLS.Service.Loans
{
    public enum LoanStatusEnum
    {
        [Description("გადაგზავნილი")]
        Sent = 1,
        [Description("დამუშავების პროცესში")]
        Processing = 2,
        [Description("დამტკიცებული")]
        Approved = 3,
        [Description("უარყოფილი")]
        Rejected = 4
    }
}
