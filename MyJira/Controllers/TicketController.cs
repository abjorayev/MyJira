using Microsoft.AspNetCore.Mvc;
using MyJira.Repository.TicketBoardRepository;
using MyJira.Services.CommentService;
using MyJira.Services.DTO;
using MyJira.Services.Helper;
using MyJira.Services.ITaskLogService;
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
        private readonly ITaskLogService _taskLogSeervice;

        public TicketController(ITicketBoardService ticketBoardService, ITicketService ticketService, ICommentService commentService,
            ITaskLogService taskLogService)
        {
            _ticketBoardService = ticketBoardService;
            _ticketService = ticketService;
            _commentService = commentService;
            _taskLogSeervice = taskLogService;
        }
        private UserProfile UserProfile => UserProfileHelper.GetUserProfile(User);
        [HttpGet]
        public async Task<IActionResult> GetComments(int ticketId)
        {
            var comments = await _commentService.GetCommentsByTicket(ticketId);
            return PartialView("_CommentsPartial", comments.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskLogs(int ticketId)
        {
            var taskLogs = await _taskLogSeervice.GetTaskLogByTicketId(ticketId);
            return PartialView("_TaskLogView", taskLogs.Data);
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
            var result = await _ticketService.Move(dto, UserProfile);
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
