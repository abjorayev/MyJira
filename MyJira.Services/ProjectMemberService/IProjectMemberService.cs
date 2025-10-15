using Microsoft.Extensions.Logging;
using MyJira.Repository.ProjectMemberRepository;
using MyJira.Services.DTO;
using MyJira.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.ProjectMemberService
{
    public interface IProjectMemberService : IMyJiraService<ProjectMemberDTO>
    {
       
    }
}
