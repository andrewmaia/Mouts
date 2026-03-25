using AutoMapper;
using Mouts.Application.Common;
using Mouts.Application.Common.Events;
using Mouts.Application.Interfaces;
using Mouts.Application.Repositories;
using Mouts.Application.UseCases.Common;
using Mouts.Domain.Exceptions;

namespace Mouts.Application.UseCases.CancelSaleItem;

public class CancelSaleItemUseCase : SaleUseCaseBase, IUseCase<CancelSaleItemRequest, CancelSaleItemResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DomainEventsDispatcher _domainEventsDispatcher;
    private readonly IMapper _mapper;

    public CancelSaleItemUseCase(ISaleRepository saleRepository, IUnitOfWork unitOfWork, DomainEventsDispatcher domainEventsDispatcher, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _domainEventsDispatcher = domainEventsDispatcher;
        _mapper = mapper;
    }

    public async Task<CancelSaleItemResponse> ExecuteAsync(CancelSaleItemRequest request)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);

        if (sale is null)
            return DomainError<CancelSaleItemResponse>($"Sale with id '{request.SaleId}' was not found.", BusinessError.NotFound);

        try
        {
            sale.CancelItem(request.ItemId);
            await _unitOfWork.CommitAsync();
            await _domainEventsDispatcher.DispatchAsync(sale.DomainEvents);
            sale.ClearDomainEvents();

            return new CancelSaleItemResponse
            {
                SaleId = sale.Id,
                ItemId = request.ItemId,
                Sale = _mapper.Map<SaleOutput>(sale)
            };
        }
        catch (SaleDomainException ex)
        {
            return DomainError<CancelSaleItemResponse>(ex);
        }
    }
}
