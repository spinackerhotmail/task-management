using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Application.UseCase.TaskEntities.Models;
using TaskManagementService.CommonLib.Exceptions;
using TaskManagementService.Domain.Events;

namespace TaskManagementService.Application.UseCase.TaskEntities.Commands.DeleteTaskEntity
{
    public class DeleteTaskEntityCommand : IRequest<TaskEntityModel>
    {
        public Guid Id { get; set; }
    }

    public class DeleteTaskEntityCommandHandler : IRequestHandler<DeleteTaskEntityCommand, TaskEntityModel>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public DeleteTaskEntityCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<TaskEntityModel> Handle(DeleteTaskEntityCommand command, CancellationToken cancellationToken)
        {
            var taskEntity = await context.TaskEntities.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
                    ?? throw new NotFoundException($"Не найдено с Id = {command.Id}");

            var resultTaskEntity = mapper.Map<TaskEntityModel>(taskEntity);

            context.TaskEntities.Remove(taskEntity);

            taskEntity.AddDomainEvent(new TaskEntityDeleted(taskEntity));
            await context.SaveChangesAsync(cancellationToken);

            return resultTaskEntity;
        }
    }


}
