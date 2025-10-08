using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Entity.DTO
{
    public class MoveTicketDTO
    {
        public int TicketId { get; set; }
        public int NewBoardId { get; set; }
    }
}
