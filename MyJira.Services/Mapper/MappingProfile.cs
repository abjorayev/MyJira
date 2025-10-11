using AutoMapper;
using MyJira.Entity.Entities;
using MyJira.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<Ticket, TicketDTO>().ReverseMap();
            CreateMap<TicketBoard, TicketBoardDTO>().ReverseMap();
        }
    }
}
