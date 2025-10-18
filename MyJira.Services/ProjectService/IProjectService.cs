using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.ProjectRepository;
using MyJira.Services.DTO;
using MyJira.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.ProjectService
{
    public interface IProjectService : IMyJiraService<ProjectDTO>
    {
        Task<OperationResult<int>> AddMemberToProject(ProjectMemberDTO  projectMemberDTO);
    }
}
