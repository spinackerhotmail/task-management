using MassTransit;
using Microsoft.Extensions.Logging;
using TaskManagementService.Domain.Events;

namespace TaskManagementService.Infrastructure.Consumers;

/// <summary>
/// Консьюмер для обработки событий обновления задачи из очереди RabbitMQ
/// </summary>
public class TaskEntityUpdatedConsumer : IConsumer<TaskEntityUpdated>
{
    private readonly ILogger<TaskEntityUpdatedConsumer> logger;

    public TaskEntityUpdatedConsumer(ILogger<TaskEntityUpdatedConsumer> logger)
    {
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<TaskEntityUpdated> context)
    {
        var message = context.Message;
        var taskEntity = message.Item;
        
        logger.LogInformation("Received TaskEntityUpdated event for TaskId: {TaskEntityId}, Title: {Title}, Status: {Status}", 
            taskEntity.Id, taskEntity.Title, taskEntity.Status);

        try
        {
            // Здесь можно добавить логику обработки события:
            // - Отправка уведомлений об изменениях
            // - Обновление кэша
            // - Аудит изменений
            // - Синхронизация с внешними системами
            
            await ProcessTaskUpdatedEvent(taskEntity);
            
            logger.LogInformation("Successfully processed TaskEntityUpdated event for TaskId: {TaskEntityId}", 
                taskEntity.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process TaskEntityUpdated event for TaskId: {TaskEntityId}", 
                taskEntity.Id);
            throw;
        }
    }

    private async Task ProcessTaskUpdatedEvent(TaskManagementService.Domain.Entities.TaskEntity taskEntity)
    {
        // Имитация обработки события
        await Task.Delay(100);

    }
}