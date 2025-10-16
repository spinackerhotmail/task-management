using FluentValidation;

namespace TaskManagementService.Application.UseCase.TaskEntities.Commands.CreateTaskEntity
{
    public class CreateTaskEntityValidator : AbstractValidator<CreateTaskEntityCommand>
    {
        public CreateTaskEntityValidator()
        {
           
        }
    }
}

