using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.DTO
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
