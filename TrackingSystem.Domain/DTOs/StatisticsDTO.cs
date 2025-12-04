namespace TrackingSystem.Domain.DTOs;

public class AttendanceStatisticsDto
{
    public int TotalLessons { get; set; }
    public int PresentCount { get; set; }
    public int AbsentCount { get; set; }
    public int LateCount { get; set; }
    public int ExcusedCount { get; set; }
    public double AttendancePercentage { get; set; }
}

public class GroupAttendanceReportDto
{
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public int TotalLessons { get; set; }
    public int PresentCount { get; set; }
    public int AbsentCount { get; set; }
    public int LateCount { get; set; }
    public int ExcusedCount { get; set; }
    public double AttendancePercentage { get; set; }
}