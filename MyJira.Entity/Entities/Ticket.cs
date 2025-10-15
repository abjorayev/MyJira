using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Entity.Entities
{
    public class Ticket : EntityBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public TicketType Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }
        public int? TicketBoardId { get; set; }
        public TicketBoard? TicketBoard { get; set; }
        public int? MemberId { get; set; }
        public Member? Member { get; set; }
    }

    public enum TicketType
    {
        Bug = 1,
        Task = 2
    }
}
