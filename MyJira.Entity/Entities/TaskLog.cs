using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Entity.Entities
{
    public class TaskLog : EntityBase
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int TicketBoardFrom { get; set; }
        public int TicketBoardTo { get; set; }
        public int TicketId {  get; set; }
        public Ticket Ticket { get; set; }
    }
}
