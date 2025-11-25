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
            CreateMap<Ticket, TicketDTO>()
           .ForMember(dest => dest.Code,
                     opt => opt.MapFrom(src => src.Project.Code))
             .ForMember(dest => dest.UserName,
             opt => opt.MapFrom(src => src.Member.Name));
            CreateMap<TicketBoard, TicketBoardDTO>().ReverseMap();
            CreateMap<ProjectMember, ProjectMemberDTO>().ReverseMap();
            CreateMap<Member, MemberDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>()
     .ForMember(dest => dest.UserName,
         opt => opt.MapFrom(src => src.Member != null ? src.Member.Name : ""));

            CreateMap<CommentDTO, Comment>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.Member, opt => opt.Ignore())
    .ForMember(dest => dest.Active, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<TaskLogDTO, TaskLog>().ReverseMap()
    .ForMember(dest => dest.MemberName,
        opt => opt.MapFrom(src => src.Member != null ? src.Member.Name : null));
        }
    }
}
