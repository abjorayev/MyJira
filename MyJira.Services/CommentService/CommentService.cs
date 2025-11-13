using AutoMapper;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.CommentRepository;
using MyJira.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private ICommentRepository _commentRepository;
        private IMapper _mapper;
        private ILogger<CommentService> _logger;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, ILogger<CommentService> logger)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> Add(CommentDTO entity)
        {
            var comment = _mapper.Map<Comment>(entity);
            comment.Active = true;
            comment.CreatedAt = DateTime.Now;
            await _commentRepository.Add(comment);
            return OperationResult<int>.Ok(entity.Id);
        }

        public async Task<OperationResult<string>> Delete(int id)
        {
            var result = await _commentRepository.Delete(id);
            if (result == false)
                return OperationResult<string>.Fail("Comment is null");

            return OperationResult<string>.Ok("Ok");
        }

        public async Task<OperationResult<List<CommentDTO>>> GetAll()
        {
            var entityComments = await _commentRepository.GetAll();
            var result = _mapper.Map<List<CommentDTO>>(entityComments);
            return OperationResult<List<CommentDTO>>.Ok(result);
        }

        public async Task<OperationResult<CommentDTO>> GetById(int id)
        {
            var comment = await _commentRepository.GetById(id);

            if (comment == null)
                return OperationResult<CommentDTO>.Fail("Comment is null");

            var result = _mapper.Map<CommentDTO>(comment);
            return OperationResult<CommentDTO>.Ok(result);
        }

        public async Task<OperationResult<List<CommentDTO>>> GetCommentsByTicket(int ticketId)
        {
            var comments = await _commentRepository.Include(x => x.Member);
            var includeComments = comments.Where(x => x.TicketId == ticketId);

            var result = _mapper.Map<List<CommentDTO>>(includeComments);
            return OperationResult<List<CommentDTO>>.Ok(result);
        }

        public async Task<OperationResult<string>> Update(CommentDTO entity)
        {
            var mapper = _mapper.Map<Comment>(entity);
            mapper.LastModifiedDate = DateTime.UtcNow;
            await _commentRepository.Update(mapper);
            return OperationResult<string>.Ok("Ok");
        }
    }
}
