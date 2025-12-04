namespace TrackingSystem.Domain.DTOs;

public class GroupDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Specialization { get; set; }
    public int Course { get; set; }
    public int StudentsCount { get; set; }
}

public class CreateGroupDto
{
    public string? Name { get; set; }
    public string? Specialization { get; set; }
    public int Course { get; set; }
}