using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MLS.Data;
using MLS.Data.Loans;
using MLS.Service.Common;
using MLS.Service.Loans.Commands;

namespace MLS.Service.Loans
{
    public sealed class LoanService : 
        IRequestHandler<CreateLoanCommand, OperationResult>, 
        IRequestHandler<UpdateLoanCommand, OperationResult>,
        IRequestHandler<GetLoanCommand, OperationResult<IEnumerable<LoanViewModel>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _userId;

        private readonly object[] _skipProps = { nameof(Loan.LoanStatusId), nameof(Loan.UserId) };

        public LoanService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userId = httpContextAccessor.HttpContext.User.FindFirst("UserId").Value;
        }

        public async Task<OperationResult> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
        {
            ValidateCreateCommand(request);

            var loanDto = _mapper.Map<CreateLoanCommand, Loan>(request);
            loanDto.UserId = _userId;

            await _unitOfWork.Loans.SaveAsync(loanDto, cancellationToken);
            _unitOfWork.Commit();

            return new OperationResult(nameof(HttpStatusCode.OK), true);
        }

        public async Task<OperationResult> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
        {
            await ValidateUpdateCommand(request);

            var loanDto = _mapper.Map<UpdateLoanCommand, Loan>(request);

            await _unitOfWork.Loans.UpdateOneAsync(x => x.Id == request.Id, loanDto, _skipProps, cancellationToken);
            _unitOfWork.Commit();

            return new OperationResult(nameof(HttpStatusCode.OK), true);
        }

        public async Task<OperationResult<IEnumerable<LoanViewModel>>> Handle(GetLoanCommand request, CancellationToken cancellationToken)
        {
            var loansDto = await _unitOfWork.Loans.FindAsync(x => x.UserId == _userId, cancellationToken: cancellationToken);
            
            var loans = _mapper.Map<IEnumerable<Loan>, IEnumerable<LoanViewModel>>(loansDto);
            return new OperationResult<IEnumerable<LoanViewModel>>(nameof(HttpStatusCode.OK), true, loans);
        }

        private void ValidateCreateCommand(CreateLoanCommand command)
        {
            if (!Enum.IsDefined(typeof(LoanTypeEnum), command.LoanTypeId))
                throw new ArgumentOutOfRangeException(nameof(command.LoanTypeId));

            if (command.Balance <= 0)
                throw new ArgumentOutOfRangeException(nameof(command.Balance));

            if (string.IsNullOrWhiteSpace(command.Ccy) || command.Ccy.Length != 3)
                throw new ArgumentNullException(nameof(command.Ccy));

            if (command.DateFrom == null || command.DateFrom.Date < DateTime.Today)
                throw new ArgumentOutOfRangeException(nameof(command.DateFrom));

            if (command.DateTo == null || command.DateTo <= command.DateFrom)
                throw new ArgumentOutOfRangeException(nameof(command.DateTo));
        }

        private async Task ValidateUpdateCommand(UpdateLoanCommand command)
        {
            if (!Enum.IsDefined(typeof(LoanTypeEnum), command.LoanTypeId))
                throw new ArgumentOutOfRangeException(nameof(command.LoanTypeId));

            if (command.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(command.Id));

            if (command.Balance <= 0)
                throw new ArgumentOutOfRangeException(nameof(command.Balance));

            if (string.IsNullOrWhiteSpace(command.Ccy) || command.Ccy.Length != 3)
                throw new ArgumentNullException(nameof(command.Ccy));

            if (command.DateFrom == null || command.DateFrom.Date < DateTime.Today)
                throw new ArgumentOutOfRangeException(nameof(command.DateFrom));

            if (command.DateTo == null || command.DateTo <= command.DateFrom)
                throw new ArgumentOutOfRangeException(nameof(command.DateTo));

            var exists = await _unitOfWork.Loans.Exists(x => x.Id == command.Id && x.UserId == _userId
                && x.LoanStatusId == (int)LoanStatusEnum.Sent || x.LoanStatusId == (int)LoanStatusEnum.Processing);
            if (!exists)
                throw new Exception("you don't have right to edit this statement");
        }
    }
}
