namespace TrackingSystem.Domain.DTOs;

public class SubjectDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public int LessonsCount { get; set; }
}

public class CreateSubjectDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int TeacherId { get; set; }
}