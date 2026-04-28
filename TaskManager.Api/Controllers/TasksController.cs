using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Models;
using TaskManager.Api.Repositories;


namespace TaskManager.Api.Controllers;

// significa que a rota é protegida e necessita de autorização
[Authorize]
// Avisa o .Net que isso é uma Api e que deve tratar erros automaticamente
[ApiController]
[Route("api/[controller]")]

public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;

    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        var tasks = await _repository.GetAllAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound(task);            
        }

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create(TaskItem task)
    {
        await _repository.AddAsync(task);

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }
}