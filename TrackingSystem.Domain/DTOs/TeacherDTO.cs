namespace TrackingSystem.Domain.DTOs;

public class TeacherDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Position { get; set; }
    public string? Email { get; set; }
    public int SubjectsCount { get; set; }
}

public class CreateTeacherDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Position { get; set; }
    public string? Email { get; set; }
}