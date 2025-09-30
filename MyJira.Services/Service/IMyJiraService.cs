using MyJira.Infastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.Service
{
    public interface IMyJiraService<T>
    {
        Task<OperationResult<T>> GetById(int id);
        Task<OperationResult<List<T>>> GetAll();
        Task<OperationResult<int>> Add(T entity);
        Task<OperationResult<string>> Update(T entity);
        Task<OperationResult<string>> Delete(int id);
    }
}
