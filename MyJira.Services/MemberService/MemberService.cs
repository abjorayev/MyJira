
using AutoMapper;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.MemberRepository;
using MyJira.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.MemberService
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MemberService> _logger;

        public MemberService(IMemberRepository memberRepository, IMapper mapper, ILogger<MemberService> logger)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> Add(MemberDTO entity)
        {
            try
            {
                var newMember = _mapper.Map<Member>(entity);
                newMember.CreatedAt = DateTime.Now;
                newMember.Active = true;
                await _memberRepository.Add(newMember);
                return OperationResult<int>.Ok(newMember.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding member: {ex.Message} {ex.StackTrace}");
                return OperationResult<int>.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<string>> Delete(int id)
        {
            try
            {
                var getMember = await _memberRepository.GetById(id);
                if (getMember == null)
                    return OperationResult<string>.Fail("Member is null");

                await _memberRepository.Delete(id);
                return OperationResult<string>.Ok(string.Empty);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while deleting Project: {ex.Message} {ex.StackTrace}");
                return OperationResult<string>.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<List<MemberDTO>>> GetAll()
        {
            var allMembers = await _memberRepository.GetAll();
            var result = _mapper.Map<List<MemberDTO>>(allMembers);
            return OperationResult<List<MemberDTO>>.Ok(result);
        }

        public async Task<OperationResult<MemberDTO>> GetById(int id)
        {
            var memberById = await _memberRepository.GetById(id);
            if(memberById == null)
                return OperationResult<MemberDTO>.Fail(string.Empty);

            var result = _mapper.Map<MemberDTO>(memberById);
            return OperationResult<MemberDTO>.Ok(result);
        }

        public async Task<OperationResult<string>> Update(MemberDTO entity)
        {
            try
            {
                var dbEntity = _mapper.Map<Member>(entity);
                dbEntity.LastModifiedDate = DateTime.Now;
                dbEntity.Name = entity.Name;
                await _memberRepository.Update(dbEntity);
                return OperationResult<string>.Ok("");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while updating Project: {ex.Message} {ex.StackTrace}");
                return OperationResult<string>.Fail(ex.Message);
            }
        }
    }
}
