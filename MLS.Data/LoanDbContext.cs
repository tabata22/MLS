using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MLS.Data.LoanEvents;
using MLS.Data.Loans;
using MLS.Data.LoanStatuses;
using MLS.Data.LoanTypes;

namespace MLS.Data
{
    public class LoanDbContext : IdentityDbContext<ApplicationUser>
    {
        public LoanDbContext(DbContextOptions<LoanDbContext> options)
            : base(options)
        {
        }

        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanEvent> LoanEvents { get; set; }
        public DbSet<LoanStatus> LoanStatuses { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
    }
}
