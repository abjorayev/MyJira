using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.ProjectMemberRepository;
using MyJira.Repository.ProjectRepository;
using MyJira.Repository.TicketRepository;
using MyJira.Services.DTO;
using MyJira.Services.ProjectMemberService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITicketRepository _ticketRepository;
        private IMapper _mapper;
        private ILogger<ProjectService> _logger;
        private readonly IProjectMemberRepository _projectMemberRepository;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper, ILogger<ProjectService> logger,
            ITicketRepository ticketRepository, IProjectMemberRepository projectMemberRepository)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _logger = logger;
            _ticketRepository = ticketRepository;
            _projectMemberRepository = projectMemberRepository;
        }

        public async Task<OperationResult<int>> Add(ProjectDTO entity)
        {
            try
            {
                var project = _mapper.Map<Project>(entity);
                project.CreatedAt = DateTime.Now;
                project.Active = true;
                await _projectRepository.Add(project);
                var projectMember = new ProjectMember
                {
                    MemberId = entity.MemberId,
                    ProjectId = project.Id,
                    CreatedAt = DateTime.Now,
                    Active = true
                };
                await _projectMemberRepository.Add(projectMember);

                return OperationResult<int>.Ok(project.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding Project: {ex.Message} {ex.StackTrace}");
                return OperationResult<int>.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<int>> AddMemberToProject(ProjectMemberDTO projectMemberDTO)
        {
            var project = _mapper.Map<ProjectMember>(projectMemberDTO);
            project.CreatedAt = DateTime.Now;
            project.Active = true;
            await _projectMemberRepository.Add(project);
            return OperationResult<int>.Ok(projectMemberDTO.Id);
        }

        public async Task<OperationResult<string>> Delete(int id)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketsByProjectId(id);
                if (tickets != null && tickets.Count > 0)
                    return OperationResult<string>.Fail("You can't delete project with tickets");

                await _projectRepository.Delete(id);
                return OperationResult<string>.Ok("");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while deleting Project: {ex.Message} {ex.StackTrace}");
                return OperationResult<string>.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<List<ProjectDTO>>> GetAll()
        {
            var projects = await _projectRepository.GetAll();
            var dtoProjects = _mapper.Map<List<ProjectDTO>>(projects);   
            return OperationResult<List<ProjectDTO>>.Ok(dtoProjects);
        }

        public async Task<OperationResult<ProjectDTO>> GetById(int id)
        {
            var project = await _projectRepository.GetById(id);
            if(project == null)
            {
                return OperationResult<ProjectDTO>.Fail("Ticket does not exist");
            }
            var dto = _mapper.Map<ProjectDTO>(project);
            return OperationResult<ProjectDTO>.Ok(dto);
        }

        public async Task<OperationResult<string>> Update(ProjectDTO entity)
        {
            try
            {
                var project = _mapper.Map<Project>(entity);
                project.LastModifiedDate = DateTime.Now;
                await _projectRepository.Update(project);
                return OperationResult<string>.Ok("Ok");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while updating Project: {ex.Message} {ex.StackTrace}");
                return OperationResult<string>.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<List<ProjectDTO>>> GetProjectByMemberId(int memberId)
        {
            var projects = _projectMemberRepository.Query().Where(x => x.MemberId == memberId).Include(x => x.Project).Select(x => x.Project).ToList();
            var dtoProjects = _mapper.Map<List<ProjectDTO>>(projects);
            return OperationResult<List<ProjectDTO>>.Ok(dtoProjects);
        }

    }
}
