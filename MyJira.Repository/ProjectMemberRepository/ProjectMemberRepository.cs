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

namespace MyJira.Repository.ProjectMemberRepository
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private ApplicationContext _context;
        private ILogger<ProjectMemberRepository> _logger;

        public ProjectMemberRepository(ApplicationContext context, ILogger<ProjectMemberRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(ProjectMember entity)
        {
            try
            {
                _context.ProjectMembers.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at adding projectMember {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var projectMember = await _context.ProjectMembers.FirstOrDefaultAsync(x => x.Id == id);
                if(projectMember == null)
                {
                    _logger.LogWarning("projectMember entity is null");
                    return false;
                }

                _context.ProjectMembers.Remove(projectMember);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at deleteing projectMember {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public Task<List<ProjectMember>> GetAll()
        {
            try
            {
                return _context.ProjectMembers.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at getting project members: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ProjectMember> GetById(int id)
        {
            try
            {
                var projectMember = await _context.ProjectMembers.FirstOrDefaultAsync(x => x.Id == id);
                if(projectMember == null)
                {
                    _logger.LogError($"Project member is null");
                    return null;
                }

                return projectMember;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at getting projectMember with id: {id} {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ProjectMember> GetFirstOrDefault(Expression<Func<ProjectMember, bool>> predicate)
        {
            return await _context.ProjectMembers.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<ProjectMember>> GetWhere(Expression<Func<ProjectMember, bool>> predicate)
        {
            return await _context.ProjectMembers.Where(predicate).ToListAsync();
        }

        public async Task Update(ProjectMember entity)
        {
            try
            {
                _context.ProjectMembers.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at updating projectMember: {ex.Message} {ex.StackTrace}");
            }
        }
    }
}
