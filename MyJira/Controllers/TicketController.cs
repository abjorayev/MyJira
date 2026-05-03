using Microsoft.AspNetCore.Mvc;
using MyJira.Repository.TicketBoardRepository;
using MyJira.Services.CommentService;
using MyJira.Services.DTO;
using MyJira.Services.Helper;
using MyJira.Services.ITaskLogService;
using MyJira.Services.ProjectMemberService;
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
        private readonly IProjectMemberService _projectMemberService;

        public TicketController(ITicketBoardService ticketBoardService, ITicketService ticketService, ICommentService commentService,
            ITaskLogService taskLogService, IProjectMemberService projectMemberService)
        {
            _ticketBoardService = ticketBoardService;
            _ticketService = ticketService;
            _commentService = commentService;
            _taskLogSeervice = taskLogService;
            _projectMemberService = projectMemberService;
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
            if (!ticketBoards.Success)
                return NotFound();
            ViewData["ProjectId"] = projectId;
            return View(ticketBoards.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Move([FromBody] MoveTicketDTO dto)
        {
            var result = await _ticketService.Move(dto, UserProfile);
            if (!result.Success)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id, int projectId)
        {
            var ticket = await _ticketService.GetTicketById(id, projectId);
            return View(ticket.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int projectId)
        {
            var ticketBoards = await _ticketBoardService.GetBoardsByProjectId(projectId);
            var members = await _projectMemberService.GetMembersByProjectId(projectId);
            ViewData["ProjectId"] = projectId;
            ViewData["TicketBoards"] = ticketBoards.Data;
            ViewData["Members"] = members.Data;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketDTO dto)
        {
            var result = await _ticketService.Add(dto);
            if (!result.Success)
                return BadRequest();
            return RedirectToAction("GetByProjectId", new { projectId = dto.ProjectId });
        }
    }
}
