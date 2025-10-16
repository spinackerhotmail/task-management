using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagementService.Domain.Events;

namespace TaskManagementService.Infrastructure.EventHandlers;

public class TaskEntityDeletedHandler: INotificationHandler<TaskEntityDeleted>
{
    private readonly ILogger<TaskEntityDeletedHandler> logger;
    private readonly IPublishEndpoint publishEndpoint;

    public TaskEntityDeletedHandler(ILogger<TaskEntityDeletedHandler> logger, IPublishEndpoint publishEndpoint)
    {
        this.logger = logger;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Handle(TaskEntityDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Task entity deleted with ID: {TaskEntityId}", notification.Item.Id);

        try
        {
            // Отправляем доменное событие в очередь RabbitMQ через MassTransit
            await publishEndpoint.Publish(notification, cancellationToken);

            logger.LogInformation("Task entity deleted event published to RabbitMQ for ID: {TaskEntityId}", notification.Item.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to publish task entity deleted event to RabbitMQ for ID: {TaskEntityId}", notification.Item.Id);
            throw;
        }
    }
}
