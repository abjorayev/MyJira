using Microsoft.AspNetCore.Mvc;
using MyJira.Repository.TicketBoardRepository;
using MyJira.Services.CommentService;
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
        private readonly ICommentService _commentService;

        public TicketController(ITicketBoardService ticketBoardService, ITicketService ticketService, ICommentService commentService)
        {
            _ticketBoardService = ticketBoardService;
            _ticketService = ticketService;
            _commentService = commentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetComments(int ticketId)
        {
            var comments = await _commentService.GetCommentsByTicket(ticketId);
            return PartialView("_CommentsPartial", comments.Data);
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
