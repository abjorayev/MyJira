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

        public async Task<OperationResult<TicketByIdDTO>> GetTicketById(int id)
        {
            var result = new TicketByIdDTO();
            var ticketAll = await _ticketRepository.Include(x => x.Project, x => x.Member);
            var ticket = ticketAll.FirstOrDefault(x => x.Id == id);
            if (ticket == null)
            {
                return OperationResult<TicketByIdDTO>.Fail("такого тикета не существует");
            }
            var ticketDTO = _mapper.Map<TicketDTO>(ticket);
            var ticketBoards = await _ticketBoardService.GetAll();
            var current = await _ticketBoardService.GetById(ticketDTO.TicketBoardId);
            result.Ticket = ticketDTO;
            result.Boards = ticketBoards.Data;
            result.CurrentBoard = current.Data;
            return OperationResult<TicketByIdDTO>.Ok(result);
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

        public async Task<OperationResult<List<TicketDTO>>> GetTicketsByBoardId(int boardId)
        {
            var entityTickets = await _ticketRepository.Include(x => x.Project, x => x.Member);
            var boardTickets = entityTickets.Where(x => x.TicketBoardId == boardId).ToList();
            var result = _mapper.Map<List<TicketDTO>>(boardTickets);
            return OperationResult<List<TicketDTO>>.Ok(result);
        }

        public async Task<OperationResult<List<TicketDTO>>> GetTicketsByProjectId(int projectId)
        {
            var entityTickets = await _ticketRepository.GetTicketsByProjectId(projectId);
            var tickets = _mapper.Map<List<TicketDTO>>(entityTickets);
            return OperationResult<List<TicketDTO>>.Ok(tickets);
        }

        public async Task<OperationResult<List<BoardTicketsDTO>>> GetBoardTicketsByProjectId(int projectId)
        {
            List<BoardTicketsDTO> boardTickets = new List<BoardTicketsDTO>();
            var boards = await _ticketBoardService.GetBoardsByProjectId(projectId);
            foreach (var board in boards.Data)
            {
                BoardTicketsDTO boardTicketsDTO = new BoardTicketsDTO();
                boardTicketsDTO.ProjectId = projectId;
                boardTicketsDTO.TicketBoardDTO = board;
                var tickets = await GetTicketsByBoardId(board.Id);
                boardTicketsDTO.Tickets = tickets.Data;
                boardTickets.Add(boardTicketsDTO);
            }
            return OperationResult<List<BoardTicketsDTO>>.Ok(boardTickets);
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
            var ticket = await _ticketRepository.GetById(dto.TicketId);
            if(ticket == null)
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
            ticket.LastModifiedDate = DateTime.UtcNow;
            ticket.TicketBoardId = dto.NewBoardId;
            await _ticketRepository.Update(ticket);
            return OperationResult<string>.Ok("OK");
        }
    }
}
