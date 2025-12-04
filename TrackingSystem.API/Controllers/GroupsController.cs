using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingSystem.Domain.DTOs;
using TrackingSystem.Domain.Entities;
using TrackingSystem.Infrastructure.Data;

namespace TrackingSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController : ControllerBase
{
    private readonly Context _context;
    private readonly IMapper _mapper;

    public GroupsController(Context context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroups()
    {
        var groups = await _context.Groups
            .Include(g => g.Students)
            .ToListAsync();

        return _mapper.Map<List<GroupDto>>(groups);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDto>> GetGroup(int id)
    {
        var group = await _context.Groups
            .Include(g => g.Students)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null) return NotFound();

        return _mapper.Map<GroupDto>(group);
    }

    [HttpPost]
    public async Task<ActionResult<GroupDto>> CreateGroup(CreateGroupDto createDto)
    {
        var group = _mapper.Map<Group>(createDto);

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGroup),
            new { id = group.Id },
            _mapper.Map<GroupDto>(group));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(int id, CreateGroupDto updateDto)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null) return NotFound();

        _mapper.Map(updateDto, group);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null) return NotFound();

        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/students")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetGroupStudents(int id)
    {
        var group = await _context.Groups
            .Include(g => g.Students)
            .ThenInclude(s => s.Group)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null) return NotFound();

        return _mapper.Map<List<StudentDto>>(group.Students);
    }
}