using Mouts.Application.Common;
using Mouts.Application.Common.Events;
using Mouts.Application.Interfaces;
using Mouts.Application.Repositories;
using Mouts.Application.UseCases.Common;
using Mouts.Domain.Exceptions;

namespace Mouts.Application.UseCases.UpdateSale;

public class UpdateSaleUseCase : SaleUseCaseBase, IUseCase<UpdateSaleRequest, UpdateSaleResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DomainEventsDispatcher _domainEventsDispatcher;

    public UpdateSaleUseCase(ISaleRepository saleRepository, IUnitOfWork unitOfWork, DomainEventsDispatcher domainEventsDispatcher)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _domainEventsDispatcher = domainEventsDispatcher;
    }

    public async Task<UpdateSaleResponse> ExecuteAsync(UpdateSaleRequest request)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);

        if (sale is null)
            return DomainError<UpdateSaleResponse>($"Sale with id '{request.SaleId}' was not found.", BusinessError.NotFound);

        try
        {
            sale.Update(
                request.SaleNumber,
                request.SaleDate,
                request.CustomerId,
                request.CustomerName,
                request.BranchId,
                request.BranchName,
                BuildSaleItems(request.Items));

            await _unitOfWork.CommitAsync();
            await _domainEventsDispatcher.DispatchAsync(sale.DomainEvents);
            sale.ClearDomainEvents();

            return new UpdateSaleResponse
            {
                SaleId = sale.Id,
                Sale = SaleOutputMapper.Map(sale)
            };
        }
        catch (SaleDomainException ex)
        {
            return DomainError<UpdateSaleResponse>(ex);
        }
    }
}
