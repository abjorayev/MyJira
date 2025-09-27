using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Entity.Entities
{
    public class EntityBase
    {
        public bool Active { get; set; } 
        public DateTime? CreatedAt { get; set; }
        public int? CreatedStaffBy { get; set; }
        public Guid? CreateUserBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
