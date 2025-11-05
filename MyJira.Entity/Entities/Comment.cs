using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Entity.Entities
{
    public class Comment : EntityBase
    {
        public int Id { get; set; } 
        public string? Text { get; set; }
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }  
    }
}
