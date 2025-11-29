using AutoMapper;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.TaskLogRepository;
using MyJira.Repository.TicketBoardRepository;
using MyJira.Repository.TicketRepository;
using MyJira.Services.DTO;
using MyJira.Services.TicketService;
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
        private ITicketBoardRepository _ticketBoardRepository;
        private ITicketRepository _ticketRepository;
        public TaskLogService(ITaskLogRepository taskLogRepository, IMapper mapper, ITicketBoardRepository ticketBoardRepository,
            ITicketRepository ticketRepository)
        {
            _taskLogRepository = taskLogRepository;
            _mapper = mapper;
            _ticketBoardRepository = ticketBoardRepository;
            _ticketRepository = ticketRepository;
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

        public async Task<OperationResult<List<GetTaskLog>>> GetTaskLogByProjectId(int projectId)
        {
            var ticket = await _ticketRepository.GetWhere(x => x.ProjectId == projectId);
            List<GetTaskLog> result = new List<GetTaskLog>();
            foreach (var entity in ticket)
            {
                var data = await GetTaskLogByTicketId(entity.Id);
                result.AddRange(data.Data);
            }
            result = result.OrderByDescending(x => x.Time).ToList();
            return OperationResult<List<GetTaskLog>>.Ok(result);
        }

        public async Task<OperationResult<List<GetTaskLog>>> GetTaskLogByTicketId(int ticketId)
        {
            var taskLogs = await _taskLogRepository.Include(x => x.Member);
            var tasks = taskLogs.Where(x => x.TicketId == ticketId);
            var task = await _ticketRepository.Include(x => x.Project);
            List<GetTaskLog> result = new List<GetTaskLog>();
            foreach(var taskLog in tasks)
            {
                var ticket = task.FirstOrDefault(x => x.Id  == ticketId);
                var ticketboardFrom = await _ticketBoardRepository.GetFirstOrDefault(x => x.Id == taskLog.TicketBoardFrom);
                var ticketboardTo = await _ticketBoardRepository.GetFirstOrDefault(x => x.Id == taskLog.TicketBoardTo);
                var newTaskLog = new GetTaskLog
                {
                    MemberName = taskLog?.Member?.Name ?? "",
                    TicketBoardTo = ticketboardTo?.Name ?? "",
                    TicketBoardFrom = ticketboardFrom?.Name ?? "",
                    Time = taskLog.CreatedAt,
                    TicketName = $"{ticket?.Project?.Code} - {ticket?.Id}" ?? "",
                    TicketId = ticket?.Id ?? 0
                };
                result.Add(newTaskLog);
            }
            result = result.OrderByDescending(x => x.Time).ToList();
            return OperationResult<List<GetTaskLog>>.Ok(result);
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
