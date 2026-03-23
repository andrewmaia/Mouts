using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Mouts.Application.Common;

namespace Mouts.Application.Execution;

public class RequestExecutor
{
    private readonly IServiceProvider _provider;

    public RequestExecutor(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request)
        where TResponse : ResultResponse, new()
    {
        //Validação (FluentValidation)
        var validators = _provider.GetServices<IValidator<TRequest>>();

        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(e => e != null)
                .ToList();

            if (failures.Any())
                return CreateErrorResponse<TResponse>(failures);
        }

        //Resolve e executa o UseCase
        var useCase = _provider.GetRequiredService<IUseCase<TRequest, TResponse>>();
        return await useCase.ExecuteAsync(request);
    }

    private static TResponse CreateErrorResponse<TResponse>(IEnumerable<ValidationFailure> failures)
        where TResponse : ResultResponse, new()
    {
        var response = new TResponse();

        foreach (var failure in failures)
            response.AddError(failure.ErrorMessage);

        return response;
    }
}
