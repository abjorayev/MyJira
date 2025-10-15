using AutoMapper;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.ProjectMemberRepository;
using MyJira.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.ProjectMemberService
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectMemberService> _logger;

        public ProjectMemberService(IProjectMemberRepository projectMemberRepository, IMapper mapper, ILogger<ProjectMemberService> logger)
        {
            _projectMemberRepository = projectMemberRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> Add(ProjectMemberDTO entity)
        {
            var mapper = _mapper.Map<ProjectMember>(entity);
            mapper.CreatedAt = DateTime.UtcNow;
            mapper.Active = true;
            await _projectMemberRepository.Add(mapper);
            return OperationResult<int>.Ok(mapper.Id);
        }

        public async Task<OperationResult<string>> Delete(int id)
        {
            var project = await _projectMemberRepository.GetById(id);
            if (project == null)
                return OperationResult<string>.Fail("ProjectMember is null");

            await _projectMemberRepository.Delete(id);
            return OperationResult<string>.Ok(string.Empty);
        }

        public async Task<OperationResult<List<ProjectMemberDTO>>> GetAll()
        {
            var dtoMembers = await _projectMemberRepository.GetAll();
            var result = _mapper.Map<List<ProjectMemberDTO>>(dtoMembers);
            return OperationResult<List<ProjectMemberDTO>>.Ok(result);
        }

        public async Task<OperationResult<ProjectMemberDTO>> GetById(int id)
        {
            var prjMembers = await _projectMemberRepository.GetById(id);
            if (prjMembers == null)
                return OperationResult<ProjectMemberDTO>.Fail("Null");

            var result = _mapper.Map<ProjectMemberDTO>(prjMembers);
            return OperationResult<ProjectMemberDTO>.Ok(result);
        }

        public async Task<OperationResult<string>> Update(ProjectMemberDTO entity)
        {
            var prjMembers = await _projectMemberRepository.GetById(entity.Id);
            if (prjMembers == null)
                return OperationResult<string>.Fail("Null");

            var result = _mapper.Map<ProjectMember>(entity);
            result.ProjectId = entity.ProjectId;
            result.MemberId = entity.MemberId;
            result.LastModifiedDate = DateTime.UtcNow;
            await _projectMemberRepository.Update(result);
            return OperationResult<string>.Ok(string.Empty);
        }
    }
}
