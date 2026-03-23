using Mouts.Application.Repositories;
using Mouts.Application.UseCases.Common;

namespace Mouts.Application.UseCases.GetSales;

public class GetSalesUseCase : IUseCase<GetSalesRequest, GetSalesResponse>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesUseCase(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<GetSalesResponse> ExecuteAsync(GetSalesRequest request)
    {
        var pagedResult = await _saleRepository.GetPagedAsync(request);

        return new GetSalesResponse
        {
            Sales = pagedResult.Items.Select(SaleOutputMapper.Map).ToList(),
            Page = pagedResult.Page,
            Size = pagedResult.Size,
            TotalCount = pagedResult.TotalCount
        };
    }
}
