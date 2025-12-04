using TrackingSystem.Domain.Entities;

namespace TrackingSystem.Domain.DTOs;

public class AttendanceDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? StudentGroup { get; set; }
    public int LessonId { get; set; }
    public string? LessonTopic { get; set; }
    public DateTime LessonDate { get; set; }
    public AttendanceStatus Status { get; set; }
    public DateTime? CheckInTime { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateAttendanceDto
{
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class MarkAttendanceDto
{
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }
}