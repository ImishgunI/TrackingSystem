using TrackingSystem.Domain.Entities;

namespace TrackingSystem.Domain.DTOs;

public class LessonDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Topic { get; set; }
    public LessonType Type { get; set; }
    public int SubjectId { get; set; }
    public string? SubjectName { get; set; }
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
    public int AttendancesCount { get; set; }
}

public class CreateLessonDto
{
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Topic { get; set; }
    public LessonType Type { get; set; }
    public int SubjectId { get; set; }
    public int GroupId { get; set; }
}