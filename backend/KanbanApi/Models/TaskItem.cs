namespace KanbanApi.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendiente"; // Pendiente | En proceso | Revisión | Finalizado
    public string Priority { get; set; } = "Media";   // Alta | Media | Baja
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }
}