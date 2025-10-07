using AutoMapper;
using Microsoft.Extensions.Logging;
using MyJira.Entity.DTO;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.TicketRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.TicketService
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper, ILogger logger)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> Add(TicketDTO entity)
        {
            var ticket = _mapper.Map<Ticket>(entity);
            ticket.CreatedAt = DateTime.Now;
            ticket.Active = true;
            await _ticketRepository.Add(ticket);
            return OperationResult<int>.Ok(ticket.Id);
        }

        public async Task<OperationResult<string>> Delete(int id)
        {
            await _ticketRepository.Delete(id);
            return OperationResult<string>.Ok("");
        }

        public async Task<OperationResult<List<TicketDTO>>> GetAll()
        {
            var entityTickets = await _ticketRepository.GetAll();
            var tickets = _mapper.Map<List<TicketDTO>>(entityTickets);
            return OperationResult<List<TicketDTO>>.Ok(tickets);
        }

        public async Task<OperationResult<TicketDTO>> GetById(int id)
        {
            var ticket = await _ticketRepository.GetById(id);
            if (ticket == null)
            {
                return OperationResult<TicketDTO>.Fail("такого тикета не существует");
            }
            var ticketDTO = _mapper.Map<TicketDTO>(ticket);
            return OperationResult<TicketDTO>.Ok(ticketDTO);
        }

        public async Task<OperationResult<List<TicketDTO>>> GetTicketsByProjectId(int projectId)
        {
            var entityTickets = await _ticketRepository.GetTicketsByProjectId(projectId);
            var tickets = _mapper.Map<List<TicketDTO>>(entityTickets);
            return OperationResult<List<TicketDTO>>.Ok(tickets);
        }

        public async Task<OperationResult<string>> Update(TicketDTO entity)
        {
            var ticket = _mapper.Map<Ticket>(entity);
            ticket.LastModifiedDate = DateTime.UtcNow;
            await _ticketRepository.Update(ticket);
            return OperationResult<string>.Ok("OK");
        }
    }
}
