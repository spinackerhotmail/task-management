using FluentValidation;

namespace TaskManagementService.Application.UseCase.TaskEntities.Commands.UpdateTaskEntity
{
    public class UpdateTaskEntityValidator: AbstractValidator<UpdateTaskEntityCommand>
    {
        public UpdateTaskEntityValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Укажите Id");

        }
    }
}
