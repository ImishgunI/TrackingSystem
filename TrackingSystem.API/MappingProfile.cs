using AutoMapper;
using TrackingSystem.Domain.DTOs;
using TrackingSystem.Domain.Entities;

namespace TrackingSystem.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Student
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group != null ? src.Group.Name : string.Empty));

        CreateMap<CreateStudentDto, Student>();

        // Group
        CreateMap<Group, GroupDto>()
            .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => src.Students != null ? src.Students.Count : 0));

        CreateMap<CreateGroupDto, Group>();

        // Teacher
        CreateMap<Teacher, TeacherDto>()
            .ForMember(dest => dest.SubjectsCount, opt => opt.MapFrom(src => src.Subjects != null ? src.Subjects.Count : 0));

        CreateMap<CreateTeacherDto, Teacher>();

        // Subject
        CreateMap<Subject, SubjectDto>()
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src =>
                src.Teacher != null ? src.Teacher.FullName : string.Empty))
            .ForMember(dest => dest.LessonsCount, opt => opt.MapFrom(src =>
                src.Lessons != null ? src.Lessons.Count : 0));

        CreateMap<CreateSubjectDto, Subject>();

        // Lesson
        CreateMap<Lesson, LessonDto>()
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src =>
                src.Subject != null ? src.Subject.Name : string.Empty))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src =>
                src.Group != null ? src.Group.Name : string.Empty))
            .ForMember(dest => dest.AttendancesCount, opt => opt.MapFrom(src =>
                src.Attendances != null ? src.Attendances.Count : 0));

        CreateMap<CreateLessonDto, Lesson>();

        // Attendance
        CreateMap<Attendance, AttendanceDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src =>
                src.Student != null ? src.Student.FullName : string.Empty))
            .ForMember(dest => dest.StudentGroup, opt => opt.MapFrom(src =>
                src.Student != null && src.Student.Group != null ? src.Student.Group.Name : string.Empty))
            .ForMember(dest => dest.LessonTopic, opt => opt.MapFrom(src =>
                src.Lesson != null ? src.Lesson.Topic : string.Empty));

        CreateMap<CreateAttendanceDto, Attendance>();
        CreateMap<MarkAttendanceDto, Attendance>();
    }
}