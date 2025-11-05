using MyJira.Entity.Entities;
using MyJira.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Repository.TicketRepository
{
    public interface ITicketRepository : IMyJiraRepository<Ticket>
    {
        Task<List<Ticket>> GetTicketsByProjectId(int projectId);
        Task<IEnumerable<Ticket>> Include(Expression<Func<Ticket, object>> predicate);
    }
}
