using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Context;
using MyJira.Infastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Repository.TaskLogRepository
{
    public class TaskLogRepository : ITaskLogRepository
    {
        private ApplicationContext _context;
        private ILogger<TaskLogRepository> _logger;

        public TaskLogRepository(ApplicationContext context, ILogger<TaskLogRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(TaskLog entity)
        {
            try
            {
                await _context.TaskLogs.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at adding Tasklog: {ex.Message} {ex.StackTrace}");
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var taskLog = await _context.TaskLogs.FirstOrDefaultAsync(x => x.Id == id);
                if(taskLog == null)
                    return false;

                _context.TaskLogs.Remove(taskLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at deleting TaskLog: {ex.Message} {ex.StackTrace}");
                return false;
            }
        }

        public Task<List<TaskLog>> GetAll()
        {
            try
            {
                return _context.TaskLogs.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at getting TaskLogs: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<TaskLog> GetById(int id)
        {
            try
            {
                var taskLog = await _context.TaskLogs.FirstOrDefaultAsync(x => x.Id == id);
                if (taskLog == null)
                    return null;

                return taskLog;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at gettimg TaskLog: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<TaskLog> GetFirstOrDefault(Expression<Func<TaskLog, bool>> predicate)
        {
            return await _context.TaskLogs.FirstOrDefaultAsync<TaskLog>(predicate);
        }

        public async Task<IEnumerable<TaskLog>> GetWhere(Expression<Func<TaskLog, bool>> predicate)
        {
            return await _context.TaskLogs.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TaskLog>> Include(params Expression<Func<TaskLog, object>>[] includes)
        {
            IQueryable<TaskLog> query = _context.TaskLogs;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task Update(TaskLog entity)
        {
            try
            {
                _context.TaskLogs.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at updating tasklog: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}
