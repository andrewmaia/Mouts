using Mouts.Application.Common;

public interface IUseCase<in TRequest, TResponse> 
    where TResponse : ResultResponse, new()
{
    Task<TResponse> ExecuteAsync(TRequest request);
}
