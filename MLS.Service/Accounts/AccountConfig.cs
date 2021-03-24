using System;
using System.Collections.Generic;
using System.Text;

namespace MLS.Service.Accounts
{
    public class AccountConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int Expires { get; set; }
    }
}
