using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.DTO
{
    public class TicketByIdDTO
    {
        public TicketDTO Ticket { get; set; }
        public List<TicketBoardDTO> Boards { get; set; } = new List<TicketBoardDTO>();
        public TicketBoardDTO CurrentBoard {  get; set; } = new TicketBoardDTO();
    }
}
