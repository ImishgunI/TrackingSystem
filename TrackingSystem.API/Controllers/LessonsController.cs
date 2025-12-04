using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingSystem.Domain.DTOs;
using TrackingSystem.Domain.Entities;
using TrackingSystem.Infrastructure.Data;

namespace TrackingSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LessonsController : ControllerBase
{
    private readonly Context _context;
    private readonly IMapper _mapper;

    public LessonsController(Context context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessons()
    {
        var lessons = await _context.Lessons
            .Include(l => l.Subject)
            .ThenInclude(s => s!.Teacher)
            .Include(l => l.Group)
            .Include(l => l.Attendances)
            .ToListAsync();

        return _mapper.Map<List<LessonDto>>(lessons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LessonDto>> GetLesson(int id)
    {
        var lesson = await _context.Lessons
            .Include(l => l.Subject)
            .ThenInclude(s => s!.Teacher)
            .Include(l => l.Group)
            .Include(l => l.Attendances)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lesson == null) return NotFound();

        return _mapper.Map<LessonDto>(lesson);
    }

    [HttpPost]
    public async Task<ActionResult<LessonDto>> CreateLesson(CreateLessonDto createDto)
    {
        var lesson = _mapper.Map<Lesson>(createDto);

        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        // Загружаем связанные данные для маппинга
        await _context.Entry(lesson).Reference(l => l.Subject).LoadAsync();
        await _context.Entry(lesson).Reference(l => l.Group).LoadAsync();

        return CreatedAtAction(nameof(GetLesson),
            new { id = lesson.Id },
            _mapper.Map<LessonDto>(lesson));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLesson(int id, CreateLessonDto updateDto)
    {
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null) return NotFound();

        _mapper.Map(updateDto, lesson);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null) return NotFound();

        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/attendance")]
    public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetLessonAttendance(int id)
    {
        var lesson = await _context.Lessons
            .Include(l => l.Attendances)
            .ThenInclude(a => a.Student)
            .ThenInclude(s => s!.Group)
            .Include(l => l.Attendances)
            .ThenInclude(a => a.Lesson)
            .ThenInclude(l => l!.Subject)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lesson == null) return NotFound();

        return _mapper.Map<List<AttendanceDto>>(lesson.Attendances);
    }

    [HttpGet("date/{date}")]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessonsByDate(DateTime date)
    {
        var lessons = await _context.Lessons
            .Include(l => l.Subject)
            .Include(l => l.Group)
            .Where(l => l.Date.Date == date.Date)
            .ToListAsync();

        return _mapper.Map<List<LessonDto>>(lessons);
    }

    [HttpGet("group/{groupId}/date/{date}")]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetGroupLessonsByDate(int groupId, DateTime date)
    {
        var lessons = await _context.Lessons
            .Include(l => l.Subject)
            .Include(l => l.Group)
            .Where(l => l.GroupId == groupId && l.Date.Date == date.Date)
            .ToListAsync();

        return _mapper.Map<List<LessonDto>>(lessons);
    }
}