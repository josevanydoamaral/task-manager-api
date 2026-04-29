
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManager.Api.Controllers;
using TaskManager.Api.Models;
using TaskManager.Api.Repositories;

namespace TaskManager.Tests;

public class TasksControllerTests
{
    // Fact é o atributo do xUnit que identifica um método de teste
    [Fact]
    public async Task GetAll_QuandoExistemTarefas_OkComLista()
    {
        // --- ARRANGE (Organizar) ---

        // Criamos o Mock do Repositório (o dublê)
        var mockRepo = new Mock<ITaskRepository>();

        // criamos uma lista de tarefas "fake" para o teste
        var fakeTasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Tarefa de Teste 1"},
            new TaskItem { Id = 2, Title = "Tarefa de Teste 2"}
        };

        // Configuramos o comportamento do Mock:
        // "Sempre que alguém chamar GetAllAsync, devolve a lista fakeTasks
        mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(fakeTasks);

        // Injetamos o Mock (através da propriedade .Object) no controller real
        var controller = new TasksController(mockRepo.Object);

        // --- ACT (Agir) ---

        // Chamamos o método que queremos testar
        var result = await controller.GetAll();

        // --- ASSERT (Afirmar/Verificar) ---

        // 1. Verificamos se o resultado é do tipo ActionResult<IEnumerable<TaskItem>>
        var okResult = Assert.IsType<OkObjectResult>(result.Result);

        // 2. Verificamos se o conteúdo dentro de OkObjectResult é a lista esperada
        var model = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(okResult.Value);

        // 3. Verificamos se a contagem de itens bate com o que criamos no Arrange
        Assert.Equal(2, model.Count());
    }
}