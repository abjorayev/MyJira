using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyJira.Entity.Entities;
using MyJira.Services.AccountService;
using MyJira.Services.ViewModel;
using System.Threading.Tasks;

namespace MyJira.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Register([FromForm] RegisterViewModel registerViewModel)
        {
            var result = await _accountService.Register(registerViewModel);
            if (!result.Success)
                return BadRequest(result.ErrorMessage);
            return RedirectToAction("Index", "Project");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var result = await _accountService.Login(loginViewModel);
            if(!result.Success)
                return BadRequest(result.ErrorMessage);

            return RedirectToAction("Index", "Project");
        }
    }
}
