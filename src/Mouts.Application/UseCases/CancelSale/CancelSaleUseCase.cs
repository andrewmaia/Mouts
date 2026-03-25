using AutoMapper;
using Mouts.Application.Common;
using Mouts.Application.Common.Events;
using Mouts.Application.Interfaces;
using Mouts.Application.Repositories;
using Mouts.Application.UseCases.Common;
using Mouts.Domain.Exceptions;

namespace Mouts.Application.UseCases.CancelSale;

public class CancelSaleUseCase : SaleUseCaseBase, IUseCase<CancelSaleRequest, CancelSaleResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DomainEventsDispatcher _domainEventsDispatcher;
    private readonly IMapper _mapper;

    public CancelSaleUseCase(ISaleRepository saleRepository, IUnitOfWork unitOfWork, DomainEventsDispatcher domainEventsDispatcher, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _domainEventsDispatcher = domainEventsDispatcher;
        _mapper = mapper;
    }

    public async Task<CancelSaleResponse> ExecuteAsync(CancelSaleRequest request)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);

        if (sale is null)
            return DomainError<CancelSaleResponse>($"Sale with id '{request.SaleId}' was not found.", BusinessError.NotFound);

        try
        {
            sale.Cancel();
            await _unitOfWork.CommitAsync();
            await _domainEventsDispatcher.DispatchAsync(sale.DomainEvents);
            sale.ClearDomainEvents();

            return new CancelSaleResponse
            {
                SaleId = sale.Id,
                Sale = _mapper.Map<SaleOutput>(sale)
            };
        }
        catch (SaleDomainException ex)
        {
            return DomainError<CancelSaleResponse>(ex);
        }
    }
}
