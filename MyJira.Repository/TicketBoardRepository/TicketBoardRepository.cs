using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Context;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Repository.TicketBoardRepository
{
    public class TicketBoardRepository : ITicketBoardRepository
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ILogger<TicketBoardRepository> _logger;

        public TicketBoardRepository(ApplicationContext applicationContext, ILogger<TicketBoardRepository> logger)
        {
            _applicationContext = applicationContext;
            _logger = logger;
        }

        public async Task Add(TicketBoard entity)
        {
            try
            {
                await _applicationContext.TicketBoards.AddAsync(entity);
                await _applicationContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at Adding TicketBoard: {ex.Message} {ex.StackTrace}");
                throw;
            }

        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var ticketBoard = await _applicationContext.TicketBoards.FirstOrDefaultAsync(x => x.Id == id);
                if(ticketBoard == null)
                {
                    _logger.LogError("Ticket board is null");
                    return false;
                }

                _applicationContext.TicketBoards.Remove(ticketBoard);
                await _applicationContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Deleting TicketBoard: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<List<TicketBoard>> GetAll()
        {
            try
            {
                return await _applicationContext.TicketBoards.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at getting all TicketBoards: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<TicketBoard> GetById(int id)
        {
            try
            {
                var ticketBoard = await _applicationContext.TicketBoards.FirstOrDefaultAsync(x => x.Id == id);
                if (ticketBoard == null)
                {
                    _logger.LogError("Ticket board is null");
                    return null;
                }

                return ticketBoard;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at getting TickerBoard by id {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<TicketBoard> GetFirstOrDefault(Expression<Func<TicketBoard, bool>> predicate)
        {
            return await _applicationContext.TicketBoards.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TicketBoard>> GetWhere(Expression<Func<TicketBoard, bool>> predicate)
        {
            return await _applicationContext.TicketBoards.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TicketBoard>> Include(params Expression<Func<TicketBoard, object>>[] includes)
        {
            IQueryable<TicketBoard> query = _applicationContext.TicketBoards;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task Update(TicketBoard entity)
        {
            try
            {
                _applicationContext.TicketBoards.Update(entity);
                await _applicationContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at updating TicketBoard {ex.Message} {ex.StackTrace}");
                throw;
            } 
        }
    }
}
