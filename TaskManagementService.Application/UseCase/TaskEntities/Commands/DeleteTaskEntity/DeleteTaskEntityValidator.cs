using FluentValidation;

namespace TaskManagementService.Application.UseCase.TaskEntities.Commands.DeleteTaskEntity
{
    public class DeleteTaskEntityValidator : AbstractValidator<DeleteTaskEntityCommand>
    {
        public DeleteTaskEntityValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Укажите Id");
        }
    }
}
