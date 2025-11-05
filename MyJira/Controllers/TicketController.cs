using Microsoft.AspNetCore.Mvc;
using MyJira.Repository.TicketBoardRepository;
using MyJira.Services.DTO;
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

        [HttpGet]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var ticketBoards = await _ticketService.GetBoardTicketsByProjectId(projectId);
            if(!ticketBoards.Success)
                return NotFound();
            return View(ticketBoards.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Move([FromBody] MoveTicketDTO dto)
        {
            var result = await _ticketService.Move(dto);
            if(!result.Success)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _ticketService.GetTicketById(id);
            return View(ticket.Data);
        }
    }
}
