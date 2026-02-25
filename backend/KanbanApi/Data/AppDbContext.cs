using Microsoft.EntityFrameworkCore;
using KanbanApi.Models;

namespace KanbanApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data inicial
        modelBuilder.Entity<TaskItem>().HasData(
            new TaskItem { Id = 1, Title = "Diseñar base de datos", Description = "Crear el esquema inicial de la base de datos.", Status = "Finalizado", Priority = "Alta", CreatedAt = DateTime.UtcNow },
            new TaskItem { Id = 2, Title = "Configurar API REST", Description = "Implementar endpoints CRUD con .NET.", Status = "En proceso", Priority = "Alta", CreatedAt = DateTime.UtcNow },
            new TaskItem { Id = 3, Title = "Crear componentes Vue", Description = "Desarrollar el tablero Kanban en Vue 3.", Status = "Pendiente", Priority = "Media", CreatedAt = DateTime.UtcNow },
            new TaskItem { Id = 4, Title = "Revisar documentación", Description = "Revisar y completar la documentación del proyecto.", Status = "Revisión", Priority = "Baja", CreatedAt = DateTime.UtcNow }
        );
    }
}