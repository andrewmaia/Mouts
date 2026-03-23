using Mouts.Application.Common;
using Mouts.Application.UseCases.GetSales;
using Mouts.Domain.Entities;

namespace Mouts.Application.Repositories;

public interface ISaleRepository
{
    void Add(Sale sale);
    Task<Sale?> GetByIdAsync(Guid id);
    Task<PagedResult<Sale>> GetPagedAsync(GetSalesRequest request);
}
