using AutoMapper;
using MLS.Data;
using MLS.Data.Loans;
using MLS.Service.Accounts.Commands;
using MLS.Service.Loans;
using MLS.Service.Loans.Commands;

namespace MLS.Service.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateLoanCommand, Loan>()
                .ForMember(dest => dest.LoanStatusId, opt 
                    => opt.MapFrom(s => LoanStatusEnum.Sent));

            CreateMap<UpdateLoanCommand, Loan>();
            CreateMap<CreateAccountCommand, ApplicationUser>();

            CreateMap<Loan, LoanViewModel>();
        }
    }
}
