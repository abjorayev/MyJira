using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.TicketRepository;
using MyJira.Services.DTO;
using MyJira.Services.ITaskLogService;
using MyJira.Services.TicketBoardService;
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
        private readonly ILogger<TicketService> _logger;
        private readonly ITicketBoardService _ticketBoardService;
        private readonly ITaskLogService.ITaskLogService _taskLogService;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper, ILogger<TicketService> logger, ITicketBoardService ticketBoardService,
            ITaskLogService.ITaskLogService taskLogService)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _logger = logger;
            _ticketBoardService = ticketBoardService;
            _taskLogService = taskLogService;
        }

        public async Task<OperationResult<TicketByIdDTO>> GetTicketById(int id, int projectId)
        {
            var result = new TicketByIdDTO();
            var ticket = _ticketRepository.Query().Include(x => x.Member).Include(x => x.Project).FirstOrDefault(x => x.Id == id);
            if (ticket == null)
            {
                _logger.LogError($"Ticket with id: {id} not found");
                return OperationResult<TicketByIdDTO>.Fail("Ticket not found");
            }
            var ticketDTO = _mapper.Map<TicketDTO>(ticket);
            var ticketBoards = await _ticketBoardService.GetBoardsByProjectId(projectId);
            var current = await _ticketBoardService.GetById(ticketDTO.TicketBoardId);
            result.Ticket = ticketDTO;
            result.Boards = ticketBoards.Data;
            result.CurrentBoard = current.Data;
            return OperationResult<TicketByIdDTO>.Ok(result);
        }

        public async Task<OperationResult<int>> Add(TicketDTO entity)
        {
            try
            {
                var ticket = _mapper.Map<Ticket>(entity);
                ticket.CreatedAt = DateTime.Now;
                ticket.Active = true;
                await _ticketRepository.Add(ticket);
                return OperationResult<int>.Ok(ticket.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding ticket: {ex.Message} {ex.StackTrace}");
                return OperationResult<int>.Fail(ex.Message);
            }
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
                _logger.LogError($"Ticket with id: {id} not found");
                return OperationResult<TicketDTO>.Fail("Ticket not found");
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

        public async Task<OperationResult<List<BoardTicketsDTO>>> GetBoardTicketsByProjectId(int projectId)
        {
            var boards = await _ticketBoardService.GetBoardsByProjectId(projectId);
            if (boards == null || boards?.Data?.Count == 0)
                return OperationResult<List<BoardTicketsDTO>>.Fail("Boards don't exist");

            var boardTickets = await _ticketRepository.GetWhere(x => boards.Data.Select(x => x.Id).Contains((int)x.TicketBoardId)).Include(x => x.Project)
               .Include(x => x.Member).ToListAsync();
            var result = boards.Data.Select(x =>
            {
                var board = boards.Data.FirstOrDefault(b => b.Id == x.Id);
                var tickets = boardTickets.Where(t => t.TicketBoardId == x.Id);
                return new BoardTicketsDTO
                {
                    ProjectId = projectId,
                    TicketBoardDTO = board,
                    Tickets = _mapper.Map<List<TicketDTO>>(tickets)
                };
            }).ToList();
      
            return OperationResult<List<BoardTicketsDTO>>.Ok(result);
        }

        public async Task<OperationResult<string>> Update(TicketDTO entity)
        {
            var ticket = _mapper.Map<Ticket>(entity);
            ticket.LastModifiedDate = DateTime.UtcNow;
            await _ticketRepository.Update(ticket);
            return OperationResult<string>.Ok("OK");
        }

        public async Task<OperationResult<string>> Move(MoveTicketDTO dto, UserProfile profile)
        {
            try
            {
                var ticket = await _ticketRepository.GetById(dto.TicketId);
                if (ticket == null)
                    return OperationResult<string>.Fail("Ticket is null");

                //Добавляем taskLog
                var taskLog = new TaskLogDTO
                {
                    TicketId = dto.TicketId,
                    TicketBoardFrom = (int)ticket.TicketBoardId,
                    TicketBoardTo = dto.NewBoardId,
                    MemberId = profile.MemberId,
                    MemberName = profile.Name
                };
                await _taskLogService.Add(taskLog);
                ticket.LastModifiedDate = DateTime.Now;
                ticket.TicketBoardId = dto.NewBoardId;
                await _ticketRepository.Update(ticket);
                return OperationResult<string>.Ok("OK");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error moving ticket with id {TicketId} to board {BoardId}", dto.TicketId, dto.NewBoardId);
                return OperationResult<string>.Fail("An error occurred while moving the ticket.");
            }
        }
    }
}
