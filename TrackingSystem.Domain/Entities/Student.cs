using System.ComponentModel.DataAnnotations;

namespace TrackingSystem.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(100)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(20)]
        public string? StudentIdNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }

}
