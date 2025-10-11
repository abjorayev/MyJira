using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.DTO
{
    public class BoardTicketsDTO
    {
        public TicketBoardDTO TicketBoardDTO {  get; set; } = new TicketBoardDTO();
        public List<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();
    }
}
