using AutoMapper;
using Microsoft.Extensions.Logging;
using MyJira.Entity.DTO;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.ProjectRepository;
using MyJira.Repository.TicketRepository;
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

        public ProjectService(IProjectRepository projectRepository, IMapper mapper, ILogger<ProjectService> logger,
            ITicketRepository ticketRepository)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _logger = logger;
            _ticketRepository = ticketRepository;
        }

        public async Task<OperationResult<int>> Add(ProjectDTO entity)
        {
            var project = _mapper.Map<Project>(entity);
            project.CreatedAt = DateTime.Now;
            project.Active = true;
            await _projectRepository.Add(project);
            return OperationResult<int>.Ok(project.Id);
        }

        public async Task<OperationResult<string>> Delete(int id)
        {
            var tickets = await _ticketRepository.GetTicketsByProjectId(id);
            if (tickets != null && tickets.Count > 0)
                return OperationResult<string>.Fail("Нельзя удалять проект с тикетами");
                
            await _projectRepository.Delete(id);
            return OperationResult<string>.Ok("");
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
                return OperationResult<ProjectDTO>.Fail("Такого проекта не существует");
            }
            var dto = _mapper.Map<ProjectDTO>(project);
            return OperationResult<ProjectDTO>.Ok(dto);
        }

        public async Task<OperationResult<string>> Update(ProjectDTO entity)
        {
            var project = _mapper.Map<Project>(entity);
            project.LastModifiedDate = DateTime.Now;
            await _projectRepository.Update(project);
            return OperationResult<string>.Ok("Ok");
        }
    }
}
