
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
        private readonly ILogger<MemberService> _memberService;

        public MemberService(IMemberRepository memberRepository, IMapper mapper, ILogger<MemberService> memberService)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
            _memberService = memberService;
        }

        public async Task<OperationResult<int>> Add(MemberDTO entity)
        {
            var newMember = _mapper.Map<Member>(entity);
            newMember.CreatedAt = DateTime.UtcNow;
            newMember.Active = true;
            await _memberRepository.Add(newMember);
            return OperationResult<int>.Ok(newMember.Id);
        }

        public async Task<OperationResult<string>> Delete(int id)
        {
            var getMember = await _memberRepository.GetById(id);
            if (getMember == null)
                return OperationResult<string>.Fail("Member is null");

            await _memberRepository.Delete(id);
            return OperationResult<string>.Ok(string.Empty);
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
            var dbEntity = _mapper.Map<Member>(entity);
            dbEntity.LastModifiedDate = DateTime.UtcNow;
            dbEntity.Name = entity.Name;
            await _memberRepository.Update(dbEntity);
            return OperationResult<string>.Ok("");
        }
    }
}
