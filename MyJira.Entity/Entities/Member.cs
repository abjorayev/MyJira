using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Entity.Entities
{
    public class Member : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
