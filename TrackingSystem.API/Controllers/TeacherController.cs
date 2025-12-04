using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingSystem.Domain.DTOs;
using TrackingSystem.Domain.Entities;
using TrackingSystem.Infrastructure.Data;

namespace TrackingSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TeachersController : ControllerBase
{
    private readonly Context _context;
    private readonly IMapper _mapper;

    public TeachersController(Context context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachers()
    {
        var teachers = await _context.Teachers
            .Include(t => t.Subjects)
            .ToListAsync();

        return _mapper.Map<List<TeacherDto>>(teachers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherDto>> GetTeacher(int id)
    {
        var teacher = await _context.Teachers
            .Include(t => t.Subjects)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (teacher == null) return NotFound();

        return _mapper.Map<TeacherDto>(teacher);
    }

    [HttpPost]
    public async Task<ActionResult<TeacherDto>> CreateTeacher(CreateTeacherDto createDto)
    {
        var teacher = _mapper.Map<Teacher>(createDto);

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeacher),
            new { id = teacher.Id },
            _mapper.Map<TeacherDto>(teacher));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(int id, CreateTeacherDto updateDto)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();

        _mapper.Map(updateDto, teacher);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/subjects")]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetTeacherSubjects(int id)
    {
        var teacher = await _context.Teachers
            .Include(t => t.Subjects)
            .ThenInclude(s => s.Teacher)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (teacher == null) return NotFound();

        return _mapper.Map<List<SubjectDto>>(teacher.Subjects);
    }
}