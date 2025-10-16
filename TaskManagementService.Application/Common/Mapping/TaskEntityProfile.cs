using AutoMapper;
using TaskManagementService.Application.UseCase.TaskEntities.Commands.CreateTaskEntity;
using TaskManagementService.Application.UseCase.TaskEntities.Models;
using TaskManagementService.Domain.Entities;

namespace TaskManagementService.Application.Common.Mapping
{
    public class TaskEntityProfile : Profile
    {
        public TaskEntityProfile()
        {
            CreateMap<CreateTaskEntityCommand, TaskEntity>();
            CreateMap<TaskEntity, TaskEntityModel>();
        }
    }
}
