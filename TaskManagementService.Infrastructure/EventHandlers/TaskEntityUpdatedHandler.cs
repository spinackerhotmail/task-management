using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagementService.Domain.Events;

namespace TaskManagementService.Infrastructure.EventHandlers;

public class TaskEntityUpdatedHandler : INotificationHandler<TaskEntityUpdated>
{
    private readonly ILogger<TaskEntityUpdatedHandler> logger;
    private readonly IPublishEndpoint publishEndpoint;

    public TaskEntityUpdatedHandler(ILogger<TaskEntityUpdatedHandler> logger, IPublishEndpoint publishEndpoint)
    {
        this.logger = logger;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Handle(TaskEntityUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Task entity updated with ID: {TaskEntityId}", notification.Item.Id);

        try
        {
            // Отправляем доменное событие в очередь RabbitMQ через MassTransit
            await publishEndpoint.Publish(notification, cancellationToken);

            logger.LogInformation("Task entity updated event published to RabbitMQ for ID: {TaskEntityId}", notification.Item.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to publish task entity updated event to RabbitMQ for ID: {TaskEntityId}", notification.Item.Id);
            throw;
        }
    }
}
