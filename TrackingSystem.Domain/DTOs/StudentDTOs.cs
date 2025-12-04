namespace TrackingSystem.Domain.DTOs;
public class StudentDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? StudentIdNumber { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
}

public class CreateStudentDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public required string StudentIdNumber { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int GroupId { get; set; }
}

public class UpdateStudentDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}