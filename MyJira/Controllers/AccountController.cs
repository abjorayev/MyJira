using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Services.AccountService;
using MyJira.Services.MemberService;
using MyJira.Services.ViewModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyJira.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;
        private readonly IMemberService _memberService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IAccountService accountService, IMemberService memberService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
            _memberService = memberService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Register([FromForm] RegisterViewModel registerViewModel)
        {
            var data = await _accountService.Register(registerViewModel);
            if (!data.Success)
                return BadRequest(data.ErrorMessage);

            var user = data.Data;
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            await _userManager.AddToRoleAsync(user, "User");
            if (!result.Succeeded)
                return RedirectToAction("NotMember");

            var claims = new List<Claim>
            {
               // new Claim(ClaimTypes.Name, user.UserName),
                new Claim("MemberId", user.MemberId.ToString()),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("MemberName", user.UserName)
            };

            await _signInManager.SignInWithClaimsAsync(user, false, claims);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
      
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel)
        {
           
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден");
                return View(loginViewModel);
            }
            var memberId = await _memberService.GetById(user.MemberId);
            if(!memberId.Success)
            {
                ModelState.AddModelError("", "Пользователь не найден");
                return View(loginViewModel);
            }
            var claims = new List<Claim>
            {
               // new Claim(ClaimTypes.Name, user.UserName),
                new Claim("MemberId", user.MemberId.ToString()),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("MemberName", memberId.Data.Name)
            };

             await _signInManager.SignInWithClaimsAsync(user, true, claims);

            //if (!result.Succeeded)
            //{
            //    ModelState.AddModelError("", "Неверный логин или пароль");
            //    return View(loginViewModel);
            //}
           
            return RedirectToAction("Index", "Project");
          
        }
    }
}
