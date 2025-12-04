using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingSystem.Domain.DTOs;
using TrackingSystem.Domain.Entities;
using TrackingSystem.Infrastructure.Data;

namespace TrackingSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly Context _context;
    private readonly IMapper _mapper;

    public SubjectsController(Context context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
    {
        var subjects = await _context.Subjects
            .Include(s => s.Teacher)
            .Include(s => s.Lessons)
            .ToListAsync();

        return _mapper.Map<List<SubjectDto>>(subjects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectDto>> GetSubject(int id)
    {
        var subject = await _context.Subjects
            .Include(s => s.Teacher)
            .Include(s => s.Lessons)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subject == null) return NotFound();

        return _mapper.Map<SubjectDto>(subject);
    }

    [HttpPost]
    public async Task<ActionResult<SubjectDto>> CreateSubject(CreateSubjectDto createDto)
    {
        var subject = _mapper.Map<Subject>(createDto);

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();

        // Загружаем связанные данные для маппинга
        await _context.Entry(subject).Reference(s => s.Teacher).LoadAsync();

        return CreatedAtAction(nameof(GetSubject),
            new { id = subject.Id },
            _mapper.Map<SubjectDto>(subject));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubject(int id, CreateSubjectDto updateDto)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();

        _mapper.Map(updateDto, subject);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/lessons")]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetSubjectLessons(int id)
    {
        var subject = await _context.Subjects
            .Include(s => s.Lessons)
            .ThenInclude(l => l.Subject)
            .Include(s => s.Lessons)
            .ThenInclude(l => l.Group)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subject == null) return NotFound();

        return _mapper.Map<List<LessonDto>>(subject.Lessons);
    }
}