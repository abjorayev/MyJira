using Microsoft.AspNetCore.Mvc;
using MyJira.Services.RoleService;
using MyJira.Services.ViewModel;

namespace MyJira.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleService _rolesService;

        public RolesController(IRoleService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var result = await _rolesService.CreateRole(roleName);
            if (!result.Success)
                return RedirectToAction("NotMember"); //Временно

            return RedirectToAction("Index", "Project"); // Тоже временно
        }

        [HttpGet]
        public IActionResult SetUserToRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetUserToRole([FromForm] SetToRoleViewModel setToRoleViewModel)
        {
            //var result = await _rolesService.SetRoleToUser(setToRoleViewModel.MemberId, setToRoleViewModel.RoleName);
          //  if (!result.Success)
             //   return RedirectToAction("NotMember"); //Временно

            return RedirectToAction("Index", "Project"); // Тоже временно
        }
    }
}
