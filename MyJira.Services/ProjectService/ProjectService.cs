using AutoMapper;
using Microsoft.Extensions.Logging;
using MyJira.Entity.DTO;
using MyJira.Entity.Entities;
using MyJira.Repository.ProjectRepository;
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
        private IMapper _mapper;
        private ILogger<ProjectService> _logger;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper, ILogger<ProjectService> logger)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Add(ProjectDTO entity)
        {
            try
            {
                var project = _mapper.Map<Project>(entity);
                project.CreatedAt = DateTime.Now;
                project.Active = true;
                await _projectRepository.Add(project);
            }
            catch(Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await _projectRepository.Delete(id);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProjectDTO>> GetAll()
        {
            try
            {
                var projects = await _projectRepository.GetAll();
                return _mapper.Map<List<ProjectDTO>>(projects);   
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProjectDTO> GetById(int id)
        {
            try
            {
                var project = await _projectRepository.GetById(id);
                return _mapper.Map<ProjectDTO>(project);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(ProjectDTO entity)
        {
            try
            {
                var project = _mapper.Map<Project>(entity);
                project.LastModifiedDate = DateTime.Now;
                await _projectRepository.Update(project);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
