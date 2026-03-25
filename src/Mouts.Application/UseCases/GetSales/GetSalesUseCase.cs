using AutoMapper;
using Mouts.Application.Repositories;
using Mouts.Application.UseCases.Common;

namespace Mouts.Application.UseCases.GetSales;

public class GetSalesUseCase : IUseCase<GetSalesRequest, GetSalesResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSalesUseCase(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<GetSalesResponse> ExecuteAsync(GetSalesRequest request)
    {
        var pagedResult = await _saleRepository.GetPagedAsync(request);

        return new GetSalesResponse
        {
            Sales = _mapper.Map<IReadOnlyCollection<SaleOutput>>(pagedResult.Items),
            Page = pagedResult.Page,
            Size = pagedResult.Size,
            TotalCount = pagedResult.TotalCount
        };
    }
}
