using MyJira.Entity.DTO;
using MyJira.Infastructure.Helper;
using MyJira.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.TicketBoardService
{
    public interface ITicketBoardService : IMyJiraService<TicketBoardDTO>
    {
        Task<OperationResult<List<TicketBoardDTO>>> GetBoardsByProjectId(int projectId);
    }
}
