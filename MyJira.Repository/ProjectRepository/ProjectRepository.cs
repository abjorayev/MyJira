using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Repository.ProjectRepository
{
    public class ProjectRepository : IProjectRepository
    {
        private ApplicationContext _context;
        private ILogger<ProjectRepository> _logger;

        public ProjectRepository(ApplicationContext context, ILogger<ProjectRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(Project entity)
        {
            try
            {
                _context.Projects.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Adding project with id:{entity.Id}, with error {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if(project == null)
                {
                    _logger.LogWarning($"Project with id:{id} not found at {DateTime.Now}");
                     return false;
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Deleting project with id:{id}, with error {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public Task<List<Project>> GetAll()
        {
            try
            {
                return _context.Projects.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Getting all projects, with error {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public Task<Project> GetById(int id)
        {
            try
            {
                var project = _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project == null)
                {
                    _logger.LogWarning($"Project with id:{id} not found at {DateTime.Now}");
                    return null;
                }

                return project;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Getting project with id:{id}, with error {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Project> GetFirstOrDefault(Expression<Func<Project, bool>> predicate)
        {
            return await _context.Projects.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Project>> GetWhere(Expression<Func<Project, bool>> predicate)
        {
            return await _context.Projects.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Project>> Include(params Expression<Func<Project, object>>[] includes)
        {
            IQueryable<Project> query = _context.Projects;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task Update(Project entity)
        {
            try
            {
                _context.Projects.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at Updating project with id:{entity.Id}, with error {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}
