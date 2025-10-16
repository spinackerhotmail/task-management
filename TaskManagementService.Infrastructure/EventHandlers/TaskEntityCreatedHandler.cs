using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagementService.Domain.Events;
using MassTransit;

namespace TaskManagementService.Infrastructure.EventHandlers;

public class TaskEntityCreatedHandler : INotificationHandler<TaskEntityCreated>
{
    private readonly ILogger<TaskEntityCreatedHandler> logger;
    private readonly IPublishEndpoint publishEndpoint;
    
    public TaskEntityCreatedHandler(ILogger<TaskEntityCreatedHandler> logger, IPublishEndpoint publishEndpoint)
    {
        this.logger = logger;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Handle(TaskEntityCreated notification, CancellationToken cancellationToken)
    {
        // Логируем создание задачи
        logger.LogInformation("Task entity created with ID: {TaskEntityId}", notification.Item.Id);

        try
        {
            // Отправляем доменное событие в очередь RabbitMQ через MassTransit
            await publishEndpoint.Publish(notification, cancellationToken);
            
            logger.LogInformation("Task entity created event published to RabbitMQ for ID: {TaskEntityId}", notification.Item.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to publish task entity created event to RabbitMQ for ID: {TaskEntityId}", notification.Item.Id);
            throw;
        }
    }
}
