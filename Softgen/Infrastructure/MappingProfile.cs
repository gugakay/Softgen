using AutoMapper;
using DataAccess.Dtos.Group;
using DataAccess.Dtos.Student;
using DataAccess.Dtos.Teacher;
using DataAccess.Entities;

namespace CarBox.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Students
            CreateMap<StudentCreateDto, StudentEntity>();
            CreateMap<StudentEntity, StudentViewDto>();
            CreateMap<StudentUpdateDto, StudentEntity>();

            //Students
            CreateMap<TeacherCreateDto, TeacherEntity>();
            CreateMap<TeacherEntity, TeacherViewDto>();
            CreateMap<TeacherUpdateDto, TeacherEntity>();

            //Students
            CreateMap<GroupCreateDto, GroupEntity>();
            CreateMap<GroupEntity, GroupViewDto>();
            CreateMap<GroupUpdateDto, GroupEntity>();
        }
    }
}
