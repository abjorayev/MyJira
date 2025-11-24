using Microsoft.AspNetCore.Mvc;
using MyJira.Services.CommentService;
using MyJira.Services.DTO;
using MyJira.Services.Helper;
using MyJira.Services.MemberService;

namespace MyJira.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
       
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        private UserProfile UserProfile => UserProfileHelper.GetUserProfile(User);
        [HttpGet]
        public IActionResult AddComment(int ticketId)
        {
            // Передаем модель с TicketId
            return PartialView("_AddComment", new CommentDTO { TicketId = ticketId });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CommentDTO commentDTO)
        {
            if (commentDTO == null || string.IsNullOrWhiteSpace(commentDTO.Text))
                return View("NotMember");

            commentDTO.UserName = UserProfile.Name;
            var result = await _commentService.Add(commentDTO);
            if(!result.Success)
                return View("NotMember");

            return Ok(); 
        }
    }
}
