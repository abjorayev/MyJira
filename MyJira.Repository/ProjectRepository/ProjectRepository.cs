using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
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
                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if(project == null)
                {
                    _logger.LogWarning($"Project with id:{id} not found at {DateTime.Now}");
                    throw new Exception($"Project with id:{id} not found");
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Deleting project with id:{id}, with error {ex.Message} {ex.StackTrace}");
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
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
                    throw new Exception($"Project with id:{id} not found");
                }

                return project;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Getting project with id:{id}, with error {ex.Message} {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
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
                throw new Exception(ex.Message);
            }
        }
    }
}
