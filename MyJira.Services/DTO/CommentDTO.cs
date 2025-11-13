using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int TicketId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UserName { get; set; }
    }
}
