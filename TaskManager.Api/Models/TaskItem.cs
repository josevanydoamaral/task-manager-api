namespace TaskManager.Api.Models;

// Esta classe representa a nossa tabela de banco de dados
public class TaskItem
{
    // O EF Core reconhece automaticamente "Id" como Chave Primária
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public bool IsCompleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}