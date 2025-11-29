using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingSystem.Domain.Entities
{
    public class Teacher
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
        [StringLength(50)]
        public string? Position { get; set; } // Должность

        [EmailAddress]
        public string? Email { get; set; }

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}
