using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Services.DTO;
using MyJira.Services.MemberService;
using MyJira.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountService> _logger;
        private readonly IMemberService _memberService;
        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountService> logger, IMemberService memberService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _memberService = memberService;
        }

        public async Task<OperationResult<string>> Login(LoginViewModel viewModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(viewModel.UserName);
                if (user == null)
                    return OperationResult<string>.Fail("User is null");

                var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, false);
                if (!result.Succeeded)
                    return OperationResult<string>.Fail("wrong login or password");
                return OperationResult<string>.Ok("Ok");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something get wrong on logining user: {ex.Message} {ex.StackTrace}");
                return OperationResult<string>.Fail("Server error");
            }
        }

        public async Task<OperationResult<string>> Register(RegisterViewModel viewModel)
        {
            try
            {
                var user = new ApplicationUser
                {
                    FullName = viewModel.FullName,
                    Email = viewModel.Email,
                    UserName = viewModel.UserName
                };

                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (!result.Succeeded)
                    return OperationResult<string>.Fail("Server error");

                await _signInManager.SignInAsync(user, false);
                var member = new MemberDTO
                {
                    UserId = new Guid(user.Id),
                    Name = user.UserName
                };
                await _memberService.Add(member);
                return OperationResult<string>.Ok("oK");

            }
            catch (Exception ex)
            {
                 _logger.LogError($"Something get wrong on creating user {ex.Message} {ex.StackTrace}");
                 return OperationResult<string>.Fail("Server error");
            }
        }
    }
}
