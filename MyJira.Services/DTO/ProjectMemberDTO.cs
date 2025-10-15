using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.DTO
{
    public class ProjectMemberDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int MemberId { get; set; }
    }
}
