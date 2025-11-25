using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Repository.TaskLogRepository
{
    public interface ITaskLogRepository : IMyJiraRepository<TaskLog>
    {
      
    }
}
