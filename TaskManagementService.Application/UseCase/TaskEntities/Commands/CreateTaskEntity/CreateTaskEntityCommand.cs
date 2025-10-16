using AutoMapper;
using MediatR;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Application.UseCase.TaskEntities.Models;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;
using TaskManagementService.Domain.Events;

namespace TaskManagementService.Application.UseCase.TaskEntities.Commands.CreateTaskEntity
{
    public class CreateTaskEntityCommand : IRequest<TaskEntityModel>
    {
        public required string UserId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; } = null!;
    }

    public class CreateTaskEntityCommandHandler : IRequestHandler<CreateTaskEntityCommand, TaskEntityModel>
    {
        private readonly IMapper mapper;
        private readonly IApplicationDbContext context;

        public CreateTaskEntityCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<TaskEntityModel> Handle(CreateTaskEntityCommand command, CancellationToken cancellationToken)
        {
            var taskEntity = mapper.Map<TaskEntity>(command);

            taskEntity.Status = TaskEntityStatus.New;

            context.TaskEntities.Add(taskEntity);

            await context.SaveChangesAsync(cancellationToken);

            taskEntity.AddDomainEvent(new TaskEntityCreated(taskEntity));

            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<TaskEntityModel>(taskEntity);
        }
    }
}
