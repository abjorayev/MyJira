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

namespace MyJira.Repository.CommentRepository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext _context;
        private ILogger<CommentRepository> _logger;

        public CommentRepository(ApplicationContext context, ILogger<CommentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Add(Comment entity)
        {
            try
            {
                await _context.Comments.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at adding comment: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var member = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
                if (member == null)
                {
                    _logger.LogWarning("Member is null at: " + DateTime.Now);
                    return false;
                }

                _context.Comments.Remove(member);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at deleting Comment: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public Task<List<Comment>> GetAll()
        {
            try
            {
                return _context.Comments.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at getting comments: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Comment> GetById(int id)
        {
            try
            {
                var member = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
                if (member == null)
                {
                    _logger.LogWarning($"Comment with id: {id} at {DateTime.Now}");
                    return null;
                }

                return member;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at getting Comment by id: {id} {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Comment> GetFirstOrDefault(Expression<Func<Comment, bool>> predicate)
        {
            return await _context.Comments.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Comment>> GetWhere(Expression<Func<Comment, bool>> predicate)
        {
            return await _context.Comments.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> Include(params Expression<Func<Comment, object>>[] includes)
        {
            IQueryable<Comment> query = _context.Comments;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task Update(Comment entity)
        {
            try
            {
                _context.Comments.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at updating comment {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}
