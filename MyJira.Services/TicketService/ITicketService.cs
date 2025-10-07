using MyJira.Entity.DTO;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.TicketService
{
    public interface ITicketService : IMyJiraService<TicketDTO>
    {
        Task<OperationResult<List<TicketDTO>>> GetTicketsByProjectId(int projectId);
    }
}
