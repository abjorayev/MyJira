using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.MemberRepository;
using MyJira.Services.DTO;
using MyJira.Services.MemberService;
using MyJira.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private readonly IConfiguration _configuration;
        private readonly IMemberRepository _memberRepository;
        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
            ILogger<AccountService> logger, IMemberService memberService, IConfiguration configuration, IMemberRepository memberRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _memberService = memberService;
            _configuration = configuration;
            _memberRepository = memberRepository;
        }

        public async Task<OperationResult<string>> Login(LoginViewModel viewModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(viewModel.UserName);
                if (user == null)
                    return OperationResult<string>.Fail("User is null");

                var result = await _signInManager.CheckPasswordSignInAsync(user, viewModel.Password, false);
                if (!result.Succeeded)
                    return OperationResult<string>.Fail("wrong login or password");


                await _signInManager.SignInAsync(user, isPersistent: false);
                //var memberId = await GetCurrentMemberId(user.Id);
                //if(memberId.Success)
                // GenerateJwtToken(user, memberId.Data);
                return OperationResult<string>.Ok("Ok");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something get wrong on logining user: {ex.Message} {ex.StackTrace}");
                return OperationResult<string>.Fail("Server error");
            }
        }

        public async Task<OperationResult<ApplicationUser>> Register(RegisterViewModel viewModel)
        {
            try
            {
                var member = new MemberDTO
                {
                    
                    Name = viewModel.UserName
                };
               var data = await _memberService.Add(member);
                var user = new ApplicationUser
                {
                    FullName = viewModel.FullName,
                    Email = viewModel.Email,
                    UserName = viewModel.UserName,
                    // MemberId = member.Id,
                    MemberId = data.Data
                };

                //var result = await _userManager.CreateAsync(user, viewModel.Password);
                //await _userManager.AddToRoleAsync(user, "User");
                //if (!result.Succeeded)
                //    return OperationResult<string>.Fail("Server error");

                //var claims = new List<Claim>
                //{
                //     new Claim(ClaimTypes.Name, user.UserName),
                //     new Claim("MemberId", user.MemberId.ToString()),
                //     new Claim(ClaimTypes.Role, "User")
                //};

                //await _signInManager.SignInWithClaimsAsync(user, false, claims);

                // GenerateJwtToken(user, member.Id);
                return OperationResult<ApplicationUser>.Ok(user);

            }
            catch (Exception ex)
            {
                 _logger.LogError($"Something get wrong on creating user {ex.Message} {ex.StackTrace}");
                 return OperationResult<ApplicationUser>.Fail("Server error");
            }
        }

        public string GenerateJwtToken(ApplicationUser user, int memberId)        
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, user.UserName),
                 new Claim("MemberId", memberId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<OperationResult<int>> GetCurrentMemberId(string userName)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
                return OperationResult<int>.Fail("User is null");
           // var member = await _memberRepository.GetFirstOrDefault(x => x.UserId.ToString() == user.Id);
         //   if (member == null)
             //   return OperationResult<int>.Fail("Member is null");

            return OperationResult<int>.Ok(0);
        }
    }
}
