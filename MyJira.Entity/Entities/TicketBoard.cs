using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Entity.Entities
{
    public class TicketBoard : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
