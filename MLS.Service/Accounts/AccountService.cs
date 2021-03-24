using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MLS.Data;
using MLS.Service.Accounts.Commands;
using MLS.Service.Common;

namespace MLS.Service.Accounts
{
    public sealed class AccountService : 
        IRequestHandler<CreateAccountCommand, OperationResult<string>>,
        IRequestHandler<LoginCommand, OperationResult<string>>,
        IRequestHandler<LogoutCommand, OperationResult>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AccountConfig _accountConfig;
        private readonly IMapper _mapper;

        public AccountService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            AccountConfig accountConfig,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _accountConfig = accountConfig;
            _mapper = mapper;
        }

        public async Task<OperationResult<string>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = _mapper.Map<CreateAccountCommand, ApplicationUser>(request);

            var result = await _userManager.CreateAsync(applicationUser, request.Password);
            if (!result.Succeeded) 
                return new OperationResult<string>("registration failed", false, null);

            var user = await _userManager.FindByNameAsync(request.UserName);
            await _signInManager.SignInAsync(user, false);

            return new OperationResult<string>(nameof(HttpStatusCode.OK), true, GenerateToken(user));
        }

        public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new OperationResult<string>("invalid username", false, null);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if (!result.Succeeded)
                return new OperationResult<string>("login failed", false, null);

            return new OperationResult<string>(nameof(HttpStatusCode.OK), true, GenerateToken(user));
        }

        public async Task<OperationResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();

            return new OperationResult(nameof(HttpStatusCode.OK), true);
        } 

        private string GenerateToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accountConfig.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserId", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_accountConfig.Issuer, _accountConfig.Issuer, claims,
                expires: DateTime.Now.AddMinutes(_accountConfig.Expires),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
