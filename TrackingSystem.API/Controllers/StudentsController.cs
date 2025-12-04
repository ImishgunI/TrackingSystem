using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingSystem.Domain.DTOs;
using TrackingSystem.Domain.Entities;
using TrackingSystem.Infrastructure.Data;

namespace TrackingSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly Context _context;
    private readonly IMapper _mapper;

    public StudentsController(Context context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
    {
        var students = await _context.Students
            .Include(s => s.Group)
            .OrderBy(s => s.Id)
            .ToListAsync();

        return _mapper.Map<List<StudentDto>>(students);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudent(int id)
    {
        var student = await _context.Students
            .Include(s => s.Group)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null) return NotFound();

        return _mapper.Map<StudentDto>(student);
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> CreateStudent(CreateStudentDto createDto)
    {
        var student = _mapper.Map<Student>(createDto);

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Перезагружаем с включением Group для маппинга
        await _context.Entry(student).Reference(s => s.Group).LoadAsync();

        return CreatedAtAction(nameof(GetStudent),
            new { id = student.Id },
            _mapper.Map<StudentDto>(student));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto updateDto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();

        _mapper.Map(updateDto, student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}