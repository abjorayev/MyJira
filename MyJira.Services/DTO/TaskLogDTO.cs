using MyJira.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.DTO
{
    public class TaskLogDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int TicketBoardFrom { get; set; }
        public int TicketBoardTo { get; set; }
        public int TicketId { get; set; }
       
    }

    public class GetTaskLog
    {
        public string MemberName { get; set; }
        public DateTime? Time { get; set; }
        public string TicketBoardFrom { get; set; }
        public string TicketBoardTo { get; set; }
    }
}
