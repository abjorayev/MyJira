using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Entity.Entities
{
    public class ProjectMember : EntityBase
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
    }
}
