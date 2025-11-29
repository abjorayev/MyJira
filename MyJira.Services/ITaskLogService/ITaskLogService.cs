using MyJira.Infastructure.Helper;
using MyJira.Services.DTO;
using MyJira.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.ITaskLogService
{
    public interface ITaskLogService : IMyJiraService<TaskLogDTO>
    {
        Task<OperationResult<List<GetTaskLog>>> GetTaskLogByTicketId(int ticketId);
    }
}
