using Microsoft.AspNetCore.Mvc;
using MyJira.Models;
using MyJira.Services.DTO;
using MyJira.Services.MemberService;
using MyJira.Services.ProjectService;
using MyJira.Services.ViewModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MyJira.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private IProjectService _projectService;
        private IMemberService _memberService; 

        public ProjectController(ILogger<ProjectController> logger, IProjectService projectService, IMemberService memberService)
        {
            _logger = logger;
            _projectService = projectService;
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAll();
            if(projects.Success)
            {
                return View(projects.Data);
            }

            return RedirectToAction("Privacy");
        }

        [HttpGet]
        public async Task<IActionResult> AddProjectMember()
        {
            var members = await _memberService.GetAll();
            var projects = await _projectService.GetAll();
            if(!projects.Success || !projects.Success)
            {
                return View("Error");
            }

            var viewModel = new ProjectMemberViewModel
            {
                MemberDTOs = members.Data,
                Projects = projects.Data
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectMember([FromForm]ProjectMemberDTO projectMemberDTO)
        {
            await _projectService.AddMemberToProject(projectMemberDTO);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
