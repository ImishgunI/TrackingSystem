using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingSystem.Domain.Entities
{
    public class Lesson
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [Required]
        [StringLength(200)]
        public string? Topic { get; set; } // Тема занятия

        public LessonType Type { get; set; } // Тип занятия (лекция, практика и т.д.)

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        // Вычисляемое свойство для даты и времени начала
        public DateTime StartDateTime => Date + StartTime;
    }
}
