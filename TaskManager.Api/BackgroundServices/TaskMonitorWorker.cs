

using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using TaskManager.Api.Data;

namespace TaskManager.Api.BackgroundServices;

public class TaskMonitorWorker: BackgroundService
{
    // ILogger serve para escrevermos no terminal de forma profissional
    private readonly ILogger<TaskMonitorWorker> _logger;

    // IServiceFactory é a "fábrica" que nos permite criar escopos manuais
    private readonly IServiceScopeFactory _scopeFactory;

    public TaskMonitorWorker(ILogger<TaskMonitorWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    // Este é o principal método que o .NET vai executar
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Enquanto a aplicação não for desligada
        while(!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker a verificar o estado das tarefas...");

            // Aplica-se a DI avançada
            // Criamos um eescopo como se fosse uma requisição fake
            using (var scope = _scopeFactory.CreateScope())
            {
                // Dentro desse escopo podemos pedirr o DBContext em segurança
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Lógica simples: contar tarefas pendentes
                var totalTasks = await context.Tasks.CountAsync(t => !t.IsCompleted);

                _logger.LogWarning($"Relatório do sistema: Existem {totalTasks} tarefas por concluir.");
            }

            // Quando sai do using, o escopo é destruído e o DbContext é fechado corretamente (SOLID)

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }


}