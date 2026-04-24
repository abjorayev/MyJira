using Microsoft.AspNetCore.Mvc;
using MyJira.Services.DTO;
using MyJira.Services.TicketBoardService;
using System.Threading.Tasks;

namespace MyJira.Controllers
{
    public class ProjectBoardController : Controller
    {
        private ITicketBoardService _ticketBoardService;

        public ProjectBoardController(ITicketBoardService ticketBoardService)
        {
            _ticketBoardService = ticketBoardService;
        }

        [HttpGet]
        public IActionResult Create(int projectId)
        {
            ViewData["ProjectId"] = projectId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketBoardDTO dto)
        {
            await _ticketBoardService.Add(dto);
            return RedirectToAction("GetByProjectId", "Ticket", new { projectId = dto.ProjectId });
        }
    }
}
