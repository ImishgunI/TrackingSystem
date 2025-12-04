using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingSystem.Domain.DTOs;
using TrackingSystem.Domain.Entities;
using TrackingSystem.Infrastructure.Data;

namespace TrackingSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AttendancesController : ControllerBase
{
    private readonly Context _context;
    private readonly IMapper _mapper;

    public AttendancesController(Context context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetAttendances()
    {
        var attendances = await _context.Attendances
            .Include(a => a.Student)
            .ThenInclude(s => s!.Group)
            .Include(a => a.Lesson)
            .ThenInclude(l => l!.Subject)
            .ToListAsync();

        return _mapper.Map<List<AttendanceDto>>(attendances);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AttendanceDto>> GetAttendance(int id)
    {
        var attendance = await _context.Attendances
            .Include(a => a.Student)
            .ThenInclude(s => s!.Group)
            .Include(a => a.Lesson)
            .ThenInclude(l => l!.Subject)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (attendance == null) return NotFound();

        return _mapper.Map<AttendanceDto>(attendance);
    }

    [HttpPost]
    public async Task<ActionResult<AttendanceDto>> CreateAttendance(CreateAttendanceDto createDto)
    {
        var attendance = _mapper.Map<Attendance>(createDto);
        attendance.CreatedAt = DateTime.UtcNow;

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        // Загружаем связанные данные для маппинга
        await _context.Entry(attendance).Reference(a => a.Student!).LoadAsync();
        await _context.Entry(attendance).Reference(a => a.Lesson!).LoadAsync();

        await _context.Entry(attendance.Student!).Reference(s => s.Group!).LoadAsync();
        await _context.Entry(attendance.Lesson!).Reference(l => l.Subject!).LoadAsync();

        return CreatedAtAction(nameof(GetAttendance),
            new { id = attendance.Id },
            _mapper.Map<AttendanceDto>(attendance));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAttendance(int id, CreateAttendanceDto updateDto)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance == null) return NotFound();

        _mapper.Map(updateDto, attendance);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttendance(int id)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance == null) return NotFound();

        _context.Attendances.Remove(attendance);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Отметить посещение
    [HttpPost("mark")]
    public async Task<ActionResult<AttendanceDto>> MarkAttendance(MarkAttendanceDto markDto)
    {
        var attendance = _mapper.Map<Attendance>(markDto);
        attendance.CheckInTime = DateTime.UtcNow;
        attendance.CreatedAt = DateTime.UtcNow;

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        // Загружаем связанные данные
        // Загружаем связанные данные для маппинга
        await _context.Entry(attendance).Reference(a => a.Student!).LoadAsync();
        await _context.Entry(attendance).Reference(a => a.Lesson!).LoadAsync();

        await _context.Entry(attendance.Student!).Reference(s => s.Group!).LoadAsync();
        await _context.Entry(attendance.Lesson!).Reference(l => l.Subject!).LoadAsync();

        return Ok(_mapper.Map<AttendanceDto>(attendance));
    }

    // Получить посещения студента
    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetStudentAttendances(int studentId)
    {
        var attendances = await _context.Attendances
            .Include(a => a.Student)
            .ThenInclude(s => s!.Group)
            .Include(a => a.Lesson)
            .ThenInclude(l => l!.Subject)
            .Where(a => a.StudentId == studentId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<AttendanceDto>>(attendances);
    }

    // Статистика посещаемости студента
    [HttpGet("statistics/student/{studentId}")]
    public async Task<ActionResult<AttendanceStatisticsDto>> GetStudentStatistics(int studentId)
    {
        var attendances = await _context.Attendances
            .Where(a => a.StudentId == studentId)
            .ToListAsync();

        var total = attendances.Count;
        var present = attendances.Count(a => a.Status == AttendanceStatus.Present);
        var absent = attendances.Count(a => a.Status == AttendanceStatus.Absent);
        var late = attendances.Count(a => a.Status == AttendanceStatus.Late);
        var excused = attendances.Count(a => a.Status == AttendanceStatus.Excused);

        var percentage = total > 0 ? (double)(present + excused) / total * 100 : 0;

        return new AttendanceStatisticsDto
        {
            TotalLessons = total,
            PresentCount = present,
            AbsentCount = absent,
            LateCount = late,
            ExcusedCount = excused,
            AttendancePercentage = Math.Round(percentage, 2)
        };
    }

    // Отчет по группе
    [HttpGet("report/group/{groupId}")]
    public async Task<ActionResult<IEnumerable<GroupAttendanceReportDto>>> GetGroupReport(int groupId)
    {
        var students = await _context.Students
            .Where(s => s.GroupId == groupId)
            .Include(s => s.Attendances)
            .ToListAsync();

        var report = students.Select(student =>
        {
            var attendances = student.Attendances;
            var total = attendances.Count;
            var present = attendances.Count(a => a.Status == AttendanceStatus.Present);
            var absent = attendances.Count(a => a.Status == AttendanceStatus.Absent);
            var late = attendances.Count(a => a.Status == AttendanceStatus.Late);
            var excused = attendances.Count(a => a.Status == AttendanceStatus.Excused);

            var percentage = total > 0 ? Math.Round((double)(present + excused) / total * 100, 2) : 0;

            return new GroupAttendanceReportDto
            {
                StudentId = student.Id,
                StudentName = student.FullName,
                TotalLessons = total,
                PresentCount = present,
                AbsentCount = absent,
                LateCount = late,
                ExcusedCount = excused,
                AttendancePercentage = percentage
            };
        }).ToList();

        return Ok(report);
    }

    // Посещаемость по дате
    [HttpGet("date/{date}")]
    public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetAttendancesByDate(DateTime date)
    {
        var attendances = await _context.Attendances
            .Include(a => a.Student)
            .ThenInclude(s => s!.Group)
            .Include(a => a.Lesson)
            .ThenInclude(l => l!.Subject)
            .Where(a => a.Lesson!.Date.Date == date.Date)
            .ToListAsync();

        return _mapper.Map<List<AttendanceDto>>(attendances);
    }
}