using AutoMapper;
using Microsoft.Extensions.Logging;
using MyJira.Entity.DTO;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.TicketBoardRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.TicketBoardService
{
    public class TicketBoardService : ITicketBoardService
    {
        private readonly ITicketBoardRepository _ticketBoardRepository;
        private readonly ILogger<TicketBoardService> _logger;
        private IMapper _mapper;

        public TicketBoardService(ITicketBoardRepository ticketBoardRepository, ILogger<TicketBoardService> logger, IMapper mapper)
        {
            _ticketBoardRepository = ticketBoardRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<OperationResult<int>> Add(TicketBoardDTO entity)
        {
            var ticketBoard = _mapper.Map<TicketBoard>(entity);
            ticketBoard.Active = true;
            ticketBoard.CreatedAt = DateTime.Now;
            await _ticketBoardRepository.Add(ticketBoard);
            
            return OperationResult<int>.Ok(entity.Id);
        }

        public async Task<OperationResult<string>> Delete(int id)
        {
            var delete = _ticketBoardRepository.GetById(id);
            if (delete == null)
                return OperationResult<string>.Fail("TicketBoard is null");

            await _ticketBoardRepository.Delete(id);
            return OperationResult<string>.Ok("");

        }

        public async Task<OperationResult<List<TicketBoardDTO>>> GetAll()
        {
            var ticketBoard = await _ticketBoardRepository.GetAll();
            var result = _mapper.Map<List<TicketBoardDTO>>(ticketBoard);
            return OperationResult<List<TicketBoardDTO>>.Ok(result);
        }

        public async Task<OperationResult<List<TicketBoardDTO>>> GetBoardsByProjectId(int projectId)
        {
            var ticketBoards = await _ticketBoardRepository.GetAll();
            var boardsProject = ticketBoards.Where(x => x.ProjectId == projectId).ToList();
            var result = _mapper.Map<List<TicketBoardDTO>>(boardsProject);
            return OperationResult<List<TicketBoardDTO>>.Ok(result);
        }

        public async Task<OperationResult<TicketBoardDTO>> GetById(int id)
        {
            var ticketBoard = await _ticketBoardRepository.GetById(id);
            if (ticketBoard == null)
                return OperationResult<TicketBoardDTO>.Fail("TicketBoard is null");

            var result = _mapper.Map<TicketBoardDTO>(ticketBoard);
            return OperationResult<TicketBoardDTO>.Ok(result);
        }

        public async Task<OperationResult<string>> Update(TicketBoardDTO entity)
        {
            var mapper = _mapper.Map<TicketBoard>(entity);
            mapper.LastModifiedDate = DateTime.Now;
            await _ticketBoardRepository.Update(mapper);
            return OperationResult<string>.Ok("");
        }
    }
}
