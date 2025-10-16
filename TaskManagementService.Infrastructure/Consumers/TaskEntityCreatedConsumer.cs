using MassTransit;
using Microsoft.Extensions.Logging;
using TaskManagementService.Domain.Events;

namespace TaskManagementService.Infrastructure.Consumers;

/// <summary>
/// Консьюмер для обработки событий создания задачи из очереди RabbitMQ
/// </summary>
public class TaskEntityCreatedConsumer : IConsumer<TaskEntityCreated>
{
    private readonly ILogger<TaskEntityCreatedConsumer> logger;

    public TaskEntityCreatedConsumer(ILogger<TaskEntityCreatedConsumer> logger)
    {
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<TaskEntityCreated> context)
    {
        var message = context.Message;
        var taskEntity = message.Item;
        
        logger.LogInformation("Received TaskEntityCreated event for TaskId: {TaskEntityId}, Title: {Title}, UserId: {UserId}", 
            taskEntity.Id, taskEntity.Title, taskEntity.UserId);

        try
        {
            // Здесь можно добавить логику обработки события:
            // - Отправка уведомлений
            // - Обновление кэша
            // - Интеграция с внешними системами
            // - Аналитика и метрики
            
            await ProcessTaskCreatedEvent(taskEntity);
            
            logger.LogInformation("Successfully processed TaskEntityCreated event for TaskId: {TaskEntityId}", 
                taskEntity.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process TaskEntityCreated event for TaskId: {TaskEntityId}", 
                taskEntity.Id);
            throw;
        }
    }

    private async Task ProcessTaskCreatedEvent(TaskManagementService.Domain.Entities.TaskEntity taskEntity)
    {
        // Имитация обработки события
        await Task.Delay(100);
        
    }
}