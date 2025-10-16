using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.CommonLib.Services;
using TaskManagementService.Domain.Events;
using TaskManagementService.Infrastructure.EventHandlers;
using TaskManagementService.Infrastructure.Persistence;
using TaskManagementService.Infrastructure.Consumers;
using MassTransit;

namespace TaskManagementService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("TmsConnection")!;

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(3);
            }));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddScoped(typeof(INotificationHandler<TaskEntityCreated>), typeof(TaskEntityCreatedHandler));
        services.AddScoped(typeof(INotificationHandler<TaskEntityUpdated>), typeof(TaskEntityUpdatedHandler));
        services.AddScoped(typeof(INotificationHandler<TaskEntityDeleted>), typeof(TaskEntityDeletedHandler));

        // Добавляем MassTransit с RabbitMQ
        services.AddMassTransit(x =>
        {
            // Регистрируем консьюмеры для доменных событий
            x.AddConsumer<TaskEntityCreatedConsumer>();
            x.AddConsumer<TaskEntityUpdatedConsumer>();
            x.AddConsumer<TaskEntityDeletedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqConnectionString = configuration.GetConnectionString("RabbitMQ") ?? "amqp://guest:guest@localhost:5672";
                cfg.Host(rabbitMqConnectionString);
                
                // Настраиваем очереди для консьюмеров доменных событий
                cfg.ReceiveEndpoint("task-entity-created", e =>
                {
                    e.ConfigureConsumer<TaskEntityCreatedConsumer>(context);
                });

                cfg.ReceiveEndpoint("task-entity-updated", e =>
                {
                    e.ConfigureConsumer<TaskEntityUpdatedConsumer>(context);
                });

                cfg.ReceiveEndpoint("task-entity-deleted", e =>
                {
                    e.ConfigureConsumer<TaskEntityDeletedConsumer>(context);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
