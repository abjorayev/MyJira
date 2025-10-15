using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Repository.MemberRepository
{
    public class MemberRepository : IMemberRepository
    {
        private ApplicationContext _context;
        private ILogger<MemberRepository> _logger;

        public MemberRepository(ApplicationContext context, ILogger<MemberRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(Member entity)
        {
            try
            {
                await _context.Members.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at adding member: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == id);
                if(member == null)
                {
                     _logger.LogWarning("Member is null at: " + DateTime.Now);
                    return false;
                }

                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at deleting member: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public Task<List<Member>> GetAll()
        {
            try
            {
                return _context.Members.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at getting members: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Member> GetById(int id)
        {
            try
            {
                var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == id);
                if(member == null)
                {
                    _logger.LogWarning($"Member with id: {id} at {DateTime.Now}");
                    return null;
                } 

                return member;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at getting member by id: {id} {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task Update(Member entity)
        {
            try
            {
                _context.Members.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at updating entity {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}
