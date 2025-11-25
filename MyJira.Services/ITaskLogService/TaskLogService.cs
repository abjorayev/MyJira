using AutoMapper;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.TaskLogRepository;
using MyJira.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.ITaskLogService
{
    public class TaskLogService : ITaskLogService
    {
        private ITaskLogRepository _taskLogRepository;
        private IMapper _mapper;

        public TaskLogService(ITaskLogRepository taskLogRepository, IMapper mapper)
        {
            _taskLogRepository = taskLogRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<int>> Add(TaskLogDTO entity)
        {
            
            var result = _mapper.Map<TaskLog>(entity);
            result.CreatedAt = DateTime.UtcNow;
            result.Active = true;
            await _taskLogRepository.Add(result);
            return OperationResult<int>.Ok(entity.Id);
           
        }

        public async Task<OperationResult<string>> Delete(int id)
        {
            var result = await _taskLogRepository.Delete(id);
            if (!result)
                return OperationResult<string>.Fail("Something got wrong");

            return OperationResult<string>.Ok(string.Empty);
        }

        public async Task<OperationResult<List<TaskLogDTO>>> GetAll()
        {
            var entities = await _taskLogRepository.GetAll();
            var result = _mapper.Map<List<TaskLogDTO>>(entities);
            return OperationResult<List<TaskLogDTO>>.Ok(result);
        }

        public async Task<OperationResult<TaskLogDTO>> GetById(int id)
        {
            var entity = await _taskLogRepository.GetById(id);
            if(entity == null)
                return OperationResult<TaskLogDTO>.Fail(string.Empty);

            var result = _mapper.Map<TaskLogDTO>(entity);
            return OperationResult<TaskLogDTO>.Ok(result);
        }

        public async Task<OperationResult<List<TaskLogDTO>>> GetTaskLogByTicketId(int ticketId)
        {
            var taskLogs = await _taskLogRepository.GetWhere(x => x.TicketId == ticketId);
            var result = _mapper.Map<List<TaskLogDTO>>(taskLogs);
            return OperationResult<List<TaskLogDTO>>.Ok(result);
        }

        public async Task<OperationResult<string>> Update(TaskLogDTO entity)
        {
            var result = _mapper.Map<TaskLog>(entity);
            result.LastModifiedDate = DateTime.Now;
            await _taskLogRepository.Update(result);
            return OperationResult<string>.Ok("");
        }

    }
}
