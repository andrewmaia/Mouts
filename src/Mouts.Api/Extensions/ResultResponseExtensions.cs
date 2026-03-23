using Mouts.Api.Contracts;
using Mouts.Application.Common;

namespace Mouts.Api.Extensions;

public static class ResultResponseExtensions
{
    public static object ToApiResponse(this ResultResponse result)
    {
        var dataProperties = result.GetType()
                                   .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                   .Where(p => p.DeclaringType != typeof(ResultResponse))
                                   .ToDictionary(p => p.Name, p => p.GetValue(result));

        return new ApiResponse
        {
            Success = result.IsSuccess,
            Data = result.IsSuccess ? dataProperties : null,
            Errors = result.IsSuccess ? null : result.Errors
        };
    }
}
