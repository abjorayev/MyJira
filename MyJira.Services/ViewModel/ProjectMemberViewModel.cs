using MyJira.Entity.Entities;
using MyJira.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.ViewModel
{
    public class ProjectMemberViewModel
    {
        public List<MemberDTO> MemberDTOs { get; set; } = new List<MemberDTO>();
        public List<ProjectDTO> Projects { get; set; } = new List<ProjectDTO>();
    }
}
