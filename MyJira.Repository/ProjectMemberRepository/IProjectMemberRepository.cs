using MyJira.Entity.Entities;
using MyJira.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Repository.ProjectMemberRepository
{
    public interface IProjectMemberRepository : IMyJiraRepository<ProjectMember>
    {
        
    }
}
