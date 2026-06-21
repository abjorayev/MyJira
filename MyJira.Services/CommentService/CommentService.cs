using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.CommentRepository;
using MyJira.Repository.MemberRepository;
using MyJira.Services.DTO;
using MyJira.Services.MemberService;
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
        private IMemberRepository _memberRepository;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, ILogger<CommentService> logger, IMemberRepository memberRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _logger = logger;
            _memberRepository = memberRepository;
        }

        public async Task<OperationResult<int>> Add(CommentDTO entity)
        {
            try
            {
                var member = _memberRepository.Query().FirstOrDefault(x => x.Name == entity.UserName);
                if (member == null)
                {
                    _logger.LogWarning("Member is null, so we can't add the comment");
                    return OperationResult<int>.Fail("Member is null");
                }
                var comment = _mapper.Map<Comment>(entity);
                comment.Active = true;
                comment.CreatedAt = DateTime.Now;
               
                comment.MemberId = member.Id;
                await _commentRepository.Add(comment);
                return OperationResult<int>.Ok(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding Coemmnt: {ex.Message} {ex.StackTrace}");
                return OperationResult<int>.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<string>> Delete(int id)
        {
            try
            {
                var result = await _commentRepository.Delete(id);
                if (result == false)
                    return OperationResult<string>.Fail("Comment is null");

                return OperationResult<string>.Ok("Ok");
            }
            catch(Exception ex) { 
            
                _logger.LogError($"Error while deleting Coemmnt: {ex.Message} {ex.StackTrace}");
                return OperationResult<string>.Fail(ex.Message);
            }
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
            var comments = _commentRepository.Query().Include(x => x.Member).Where(x => x.TicketId == ticketId);
           
            var result = _mapper.Map<List<CommentDTO>>(comments);
            return OperationResult<List<CommentDTO>>.Ok(result);
        }

        public async Task<OperationResult<string>> Update(CommentDTO entity)
        {
            try
            {
                var mapper = _mapper.Map<Comment>(entity);
                mapper.LastModifiedDate = DateTime.Now;
                await _commentRepository.Update(mapper);
                return OperationResult<string>.Ok("Ok");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding Coemmnt: {ex.Message} {ex.StackTrace}");
                return OperationResult<string>.Fail(ex.Message);
            }
        }
    }
}
