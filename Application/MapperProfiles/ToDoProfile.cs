using AutoMapper;
using ToDoWeb.Application.Dtos;
using ToDoWeb.Domains.Entities;

namespace ToDoWeb.Application.MapperProfiles
{
    public class ToDoProfile : Profile
    {

        public ToDoProfile()
        {
            //Map Course to CourseViewModel

            CreateMap<Course, CourseViewModel>()
                .ForMember(dest => dest.CourseId, config => config.MapFrom(src => src.Id))
                .ForMember(dest => dest.CourseName, config => config.MapFrom(src => src.Name))
                .ReverseMap();

            //Nếu cần map cả 2 chiều thì .reverseMap();


            CreateMap<CourseCreateModel, Course>();

            CreateMap<CourseUpdateModel, Course>()
                .ForMember(dest => dest.Id, config => config.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.Name, config =>
                {
                    config.PreCondition(src => !string.IsNullOrEmpty(src.CourseName));
                    config.MapFrom(src => src.CourseName);
                });

            CreateMap<Student, StudentViewModel>()
                .ForMember(dest => dest.FullName, config => config.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.SchoolName, config => config.MapFrom(src => src.School.Name));

            CreateMap<Student, StudentCourseViewModel>()
                .ForMember(dest => dest.StudentId, config => config.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentName, config => config.MapFrom(src => src.FirstName + " " + src.LastName));

            CreateMap<CourseStudent, CourseViewModel>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Course.Id))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Course.StartDate));

            CreateMap<Student, StudentCourseViewModel>()
                .ForMember(dest => dest.StudentId, config => config.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentName, config => config.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Course, config => config.MapFrom(src => src.CourseStudents.Select(x => x.Course)));


        }
    }
}
