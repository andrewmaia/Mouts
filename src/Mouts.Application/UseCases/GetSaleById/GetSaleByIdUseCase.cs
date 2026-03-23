using Mouts.Application.Common;
using Mouts.Application.Repositories;
using Mouts.Application.UseCases.Common;

namespace Mouts.Application.UseCases.GetSaleById;

public class GetSaleByIdUseCase : SaleUseCaseBase, IUseCase<GetSaleByIdRequest, GetSaleByIdResponse>
{
    private readonly ISaleRepository _saleRepository;

    public GetSaleByIdUseCase(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<GetSaleByIdResponse> ExecuteAsync(GetSaleByIdRequest request)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);

        if (sale is null)
            return DomainError<GetSaleByIdResponse>($"Sale with id '{request.SaleId}' was not found.", BusinessError.NotFound);

        return new GetSaleByIdResponse
        {
            Sale = SaleOutputMapper.Map(sale)
        };
    }
}
