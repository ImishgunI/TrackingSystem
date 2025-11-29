using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingSystem.Domain.Entities
{
    public enum AttendanceStatus
    {
        Present = 1,    // Присутствовал
        Absent = 2,     // Отсутствовал
        Late = 3,       // Опоздал
        Excused = 4     // Отсутствие по уважительной причине
    }
}
