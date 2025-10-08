using Microsoft.AspNetCore.Mvc;
using MyJira.Repository.TicketBoardRepository;
using MyJira.Services.TicketBoardService;
using MyJira.Services.TicketService;
using System.Threading.Tasks;

namespace MyJira.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketBoardService _ticketBoardService;
        private readonly ITicketService _ticketService;

        public TicketController(ITicketBoardService ticketBoardService, ITicketService ticketService)
        {
            _ticketBoardService = ticketBoardService;
            _ticketService = ticketService;
        }

        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var ticketBoards = await _ticketService.GetBoardTicketsByProjectId(projectId);
            if(!ticketBoards.Success)
                return NotFound();
            return View(ticketBoards.Data);
        }
    }
}
