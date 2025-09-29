using AutoMapper;
using MyJira.Entity.DTO;
using MyJira.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Infastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<Ticket, TicketDTO>().ReverseMap();
            CreateMap <List<Project>, List<ProjectDTO>>().ReverseMap();
            CreateMap<List<Ticket>, List<TicketDTO>>().ReverseMap();
        }
    }
}
