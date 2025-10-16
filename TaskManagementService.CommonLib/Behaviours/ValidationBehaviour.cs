using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManagementService.CommonLib.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IServiceProvider serviceProvider;

        public ValidationBehaviour(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> nextAction,
            CancellationToken cancellationToken)
        {
            var validator = serviceProvider.GetService<IValidator<TRequest>>();

            if (validator == null)
            {
                return await nextAction();
            }

            await validator.ValidateAndThrowAsync(request, cancellationToken);

            return await nextAction();
        }
    }
}
