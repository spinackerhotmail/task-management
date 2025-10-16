using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Application.UseCase.TaskEntities.Models;
using TaskManagementService.CommonLib.Exceptions;
using TaskManagementService.Domain.Events;

namespace TaskManagementService.Application.UseCase.TaskEntities.Commands.UpdateTaskEntity
{
    public class UpdateTaskEntityCommand : IRequest<TaskEntityModel>
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; } = null!;
    }

    public class UpdateTaskEntityCommandHandler : IRequestHandler<UpdateTaskEntityCommand, TaskEntityModel>
    {
        private readonly IMapper mapper;
        private readonly IApplicationDbContext context;

        public UpdateTaskEntityCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<TaskEntityModel> Handle(UpdateTaskEntityCommand command, CancellationToken cancellationToken)
        {
            var taskEntity = await context.TaskEntities.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
                ?? throw new NotFoundException($"Не найдено с Id = {command.Id}");

            if(!string.IsNullOrWhiteSpace(command.Title))
                taskEntity.Title = command.Title;

            if (command.Description != null)
                taskEntity.Description = command.Description;


            taskEntity.AddDomainEvent(new TaskEntityUpdated(taskEntity));
            
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<TaskEntityModel>(taskEntity);
        }
    }

}
