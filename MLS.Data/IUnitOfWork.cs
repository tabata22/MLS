using MLS.Data.LoanEvents;
using MLS.Data.Loans;

namespace MLS.Data
{
    public interface IUnitOfWork
    {
        public IBaseRepository<Loan> Loans { get; }
        public IBaseRepository<LoanEvent> LoanEvents { get; }

        public void Commit();
    }
}
