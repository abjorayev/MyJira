using Microsoft.AspNetCore.Mvc;
using MyJira.Services.ITaskLogService;
using System.Threading.Tasks;

namespace MyJira.Controllers
{
    public class TaskLogController : Controller
    {
        private ITaskLogService _taskLogService;

        public TaskLogController(ITaskLogService taskLogService)
        {
            _taskLogService = taskLogService;
        }

        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var taskLogs = await _taskLogService.GetTaskLogByProjectId(projectId);
            return View("~/Views/Ticket/_TaskLogView.cshtml", taskLogs.Data);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
