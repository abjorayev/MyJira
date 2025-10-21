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
      
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel)
        {
           
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден");
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                loginViewModel.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(loginViewModel);
            }
           
            return RedirectToAction("Index", "Project");
          
        }
    }
}
