using Microsoft.EntityFrameworkCore;
using Mouts.Application.Common;
using Mouts.Application.Repositories;
using Mouts.Application.UseCases.GetSales;
using Mouts.Domain.Entities;
using Mouts.Infrastructure.PostgreSQL.Context;

namespace Mouts.Infrastructure.PostgreSQL.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly MoutsDbContext _db;

    public SaleRepository(MoutsDbContext db)
    {
        _db = db;
    }

    public void Add(Sale sale)
    {
        _db.Sales.Add(sale);
    }

    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _db.Sales
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedResult<Sale>> GetPagedAsync(GetSalesRequest request)
    {
        var query = _db.Sales
            .Include(x => x.Items)
            .AsQueryable();

        query = ApplyFilters(query, request);
        query = ApplySorting(query, request.Order);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync();

        return new PagedResult<Sale>
        {
            Items = items,
            Page = request.Page,
            Size = request.Size,
            TotalCount = totalCount
        };
    }

    private static IQueryable<Sale> ApplyFilters(IQueryable<Sale> query, GetSalesRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.SaleNumber))
        {
            var saleNumberFilter = NormalizeLikePattern(request.SaleNumber!);
            query = request.SaleNumber!.Contains('*')
                ? query.Where(x => EF.Functions.Like(x.SaleNumber, saleNumberFilter))
                : query.Where(x => x.SaleNumber == request.SaleNumber);
        }

        if (!string.IsNullOrWhiteSpace(request.CustomerName))
        {
            var customerNameFilter = NormalizeLikePattern(request.CustomerName!);
            query = request.CustomerName!.Contains('*')
                ? query.Where(x => EF.Functions.Like(x.CustomerName, customerNameFilter))
                : query.Where(x => x.CustomerName == request.CustomerName);
        }

        if (!string.IsNullOrWhiteSpace(request.BranchName))
        {
            var branchNameFilter = NormalizeLikePattern(request.BranchName!);
            query = request.BranchName!.Contains('*')
                ? query.Where(x => EF.Functions.Like(x.BranchName, branchNameFilter))
                : query.Where(x => x.BranchName == request.BranchName);
        }

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        if (request.SaleDateMin.HasValue)
            query = query.Where(x => x.SaleDate >= request.SaleDateMin.Value);

        if (request.SaleDateMax.HasValue)
            query = query.Where(x => x.SaleDate <= request.SaleDateMax.Value);

        if (request.TotalAmountMin.HasValue)
            query = query.Where(x => x.TotalAmount >= request.TotalAmountMin.Value);

        if (request.TotalAmountMax.HasValue)
            query = query.Where(x => x.TotalAmount <= request.TotalAmountMax.Value);

        return query;
    }

    private static IQueryable<Sale> ApplySorting(IQueryable<Sale> query, string? order)
    {
        var normalizedOrder = order?.Trim().ToLowerInvariant();

        return normalizedOrder switch
        {
            "salenumber" => query.OrderBy(x => x.SaleNumber),
            "-salenumber" => query.OrderByDescending(x => x.SaleNumber),
            "customername" => query.OrderBy(x => x.CustomerName),
            "-customername" => query.OrderByDescending(x => x.CustomerName),
            "branchname" => query.OrderBy(x => x.BranchName),
            "-branchname" => query.OrderByDescending(x => x.BranchName),
            "totalamount" => query.OrderBy(x => x.TotalAmount),
            "-totalamount" => query.OrderByDescending(x => x.TotalAmount),
            "saledate" => query.OrderBy(x => x.SaleDate),
            "-saledate" => query.OrderByDescending(x => x.SaleDate),
            _ => query.OrderByDescending(x => x.SaleDate).ThenBy(x => x.SaleNumber)
        };
    }

    private static string NormalizeLikePattern(string filter) => filter.Replace('*', '%');
}
