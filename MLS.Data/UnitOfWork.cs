using MLS.Data.LoanEvents;
using MLS.Data.Loans;

namespace MLS.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LoanDbContext _context;
        private IBaseRepository<Loan> _loansRepository;
        private IBaseRepository<LoanEvent> _loanEventsRepository;

        public UnitOfWork(LoanDbContext context) => _context = context;

        public IBaseRepository<Loan> Loans
        {
            get { return _loansRepository ??= new BaseRepository<Loan>(_context); }
        }

        public IBaseRepository<LoanEvent> LoanEvents
        {
            get { return _loanEventsRepository ??= new BaseRepository<LoanEvent>(_context); }
        }

        public void Commit() => _context.SaveChanges();
    }
}
