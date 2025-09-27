using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Repository.TicketRepository
{
    public class TicketRepository : ITicketRepository
    {
        private ApplicationContext _context;
        private ILogger<TicketRepository> _logger;

        public TicketRepository(ApplicationContext context, ILogger<TicketRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(Ticket entity)
        {
            try
            {
                await _context.Tickets.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Adding ticket with id:{entity.Id}, with error {ex.Message} {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var ticket = await _context.Tickets.FirstOrDefaultAsync(p => p.Id == id);
                if(ticket == null)
                {
                    _logger.LogWarning($"Ticket with id:{id} not found at {DateTime.Now}");
                    throw new Exception($"Ticket with id:{id} not found");
                }

                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Deleting ticket with id:{id}, with error {ex.Message} {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Ticket>> GetAll()
        {
            try
            {
                return await _context.Tickets.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Getting all tickets, with error {ex.Message} {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<Ticket> GetById(int id)
        {
            try
            {
                var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
                if(ticket == null)
                {
                    _logger.LogWarning($"Ticket with id:{id} not found at {DateTime.Now}");
                    throw new Exception($"Ticket with id:{id} not found");
                }

                return ticket;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Getting ticket with id:{id}, with error {ex.Message} {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Ticket>> GetTicketsByProjectId(int projectId)
        {
            try
            {
                return await _context.Tickets.Where(t => t.ProjectId == projectId).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error at Getting tickets by project id:{projectId}, with error {ex.Message} {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(Ticket entity)
        {
            try
            {
                 _context.Tickets.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at Updating ticket with id:{entity.Id}, with error {ex.Message} {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }
    }
}
