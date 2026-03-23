using Mouts.Application.Common.Events;
using Mouts.Application.Interfaces;
using Mouts.Application.Repositories;
using Mouts.Application.UseCases.Common;
using Mouts.Domain.Entities;
using Mouts.Domain.Exceptions;

namespace Mouts.Application.UseCases.CreateSale;

public class CreateSaleUseCase : SaleUseCaseBase, IUseCase<CreateSaleRequest, CreateSaleResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DomainEventsDispatcher _domainEventsDispatcher;

    public CreateSaleUseCase(ISaleRepository saleRepository, IUnitOfWork unitOfWork, DomainEventsDispatcher domainEventsDispatcher)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _domainEventsDispatcher = domainEventsDispatcher;
    }

    public async Task<CreateSaleResponse> ExecuteAsync(CreateSaleRequest request)
    {
        try
        {
            var sale = new Sale(
                request.SaleNumber,
                request.SaleDate,
                request.CustomerId,
                request.CustomerName,
                request.BranchId,
                request.BranchName,
                BuildSaleItems(request.Items));

            _saleRepository.Add(sale);
            await _unitOfWork.CommitAsync();
            await _domainEventsDispatcher.DispatchAsync(sale.DomainEvents);
            sale.ClearDomainEvents();

            return new CreateSaleResponse
            {
                SaleId = sale.Id,
                Sale = SaleOutputMapper.Map(sale)
            };
        }
        catch (SaleDomainException ex)
        {
            return DomainError<CreateSaleResponse>(ex);
        }
    }
}
