using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KanbanApi.Data;
using KanbanApi.DTOs;
using KanbanApi.Models;

namespace KanbanApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        return await _context.Tasks.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    // GET: api/tasks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        return task;
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create(CreateTaskDto dto)
    {
        var validStatuses = new[] { "Pendiente", "En proceso", "Revisión", "Finalizado" };
        if (!validStatuses.Contains(dto.Status))
            return BadRequest("Estado no válido.");

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            FinishedAt = dto.FinishedAt,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    // PUT: api/tasks/5
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> Update(int id, UpdateTaskDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        if (dto.Title != null) task.Title = dto.Title;
        if (dto.Description != null) task.Description = dto.Description;
        if (dto.Status != null) task.Status = dto.Status;
        if (dto.Priority != null) task.Priority = dto.Priority;
        if (dto.FinishedAt.HasValue) task.FinishedAt = dto.FinishedAt;

        await _context.SaveChangesAsync();
        return task;
    }

    // PATCH: api/tasks/5/status  ← Para drag & drop
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<TaskItem>> UpdateStatus(int id, UpdateStatusDto dto)
    {
        var validStatuses = new[] { "Pendiente", "En proceso", "Revisión", "Finalizado" };
        if (!validStatuses.Contains(dto.Status))
            return BadRequest("Estado no válido.");

        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        task.Status = dto.Status;

        if (dto.Status == "Finalizado" && task.FinishedAt == null)
            task.FinishedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return task;
    }

    // DELETE: api/tasks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}