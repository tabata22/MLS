using System;

namespace MLS.Data.LoanEvents
{
    public class LoanEvent
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public bool Processed { get; set; }
        public DateTime Created { get; set; }
    }
}
