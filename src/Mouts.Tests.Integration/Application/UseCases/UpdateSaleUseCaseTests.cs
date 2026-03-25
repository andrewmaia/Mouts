using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mouts.Application.Common.Events;
using Mouts.Application.Mapping;
using Mouts.Application.UseCases.Common;
using Mouts.Application.UseCases.CreateSale;
using Mouts.Application.UseCases.GetSaleById;
using Mouts.Application.UseCases.UpdateSale;
using Mouts.Infrastructure.PostgreSQL.Context;
using Mouts.Infrastructure.PostgreSQL.Repositories;
using Mouts.Tests.Integration.Application.Fixtures;

namespace Mouts.Tests.Integration.Application.UseCases;

public class UpdateSaleUseCaseTests
{
    [Fact]
    public async Task Should_UpdateSaleSuccessfully_InMemory()
    {
        var options = new DbContextOptionsBuilder<MoutsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new MoutsDbContext(options);

        var saleRepository = new SaleRepository(context);
        var unitOfWork = new UnitOfWorkInMemory(context);
        var serviceProvider = BuildServiceProvider();
        var dispatcher = new DomainEventsDispatcher(serviceProvider);
        var mapper = serviceProvider.GetRequiredService<IMapper>();

        var createUseCase = new CreateSaleUseCase(saleRepository, unitOfWork, dispatcher, mapper);
        var updateUseCase = new UpdateSaleUseCase(saleRepository, unitOfWork, dispatcher, mapper);
        var getByIdUseCase = new GetSaleByIdUseCase(saleRepository, mapper);

        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productA = Guid.NewGuid();
        var productB = Guid.NewGuid();

        var createResponse = await createUseCase.ExecuteAsync(new CreateSaleRequest
        {
            SaleNumber = "S-2000",
            SaleDate = DateTime.UtcNow,
            CustomerId = customerId,
            CustomerName = "Customer A",
            BranchId = branchId,
            BranchName = "Branch A",
            Items =
            [
                new SaleItemInput
                {
                    ProductId = productA,
                    ProductName = "Notebook",
                    Quantity = 2,
                    UnitPrice = 100m
                },
                new SaleItemInput
                {
                    ProductId = productB,
                    ProductName = "Mouse",
                    Quantity = 1,
                    UnitPrice = 50m
                }
            ]
        });

        var originalSale = await getByIdUseCase.ExecuteAsync(new GetSaleByIdRequest
        {
            SaleId = createResponse.SaleId!.Value
        });

        var originalItemId = originalSale.Sale!.Items.Single(x => x.ProductId == productA).Id;

        var updateResponse = await updateUseCase.ExecuteAsync(new UpdateSaleRequest
        {
            SaleId = createResponse.SaleId.Value,
            SaleNumber = "S-2000-UPDATED",
            SaleDate = DateTime.UtcNow,
            CustomerId = customerId,
            CustomerName = "Customer A Updated",
            BranchId = branchId,
            BranchName = "Branch A",
            Items =
            [
                new SaleItemInput
                {
                    ProductId = productA,
                    ProductName = "Notebook",
                    Quantity = 4,
                    UnitPrice = 100m
                },
                new SaleItemInput
                {
                    ProductId = productB,
                    ProductName = "Mouse",
                    Quantity = 1,
                    UnitPrice = 50m
                }
            ]
        });

        Assert.True(updateResponse.IsSuccess);

        var updatedSale = await getByIdUseCase.ExecuteAsync(new GetSaleByIdRequest
        {
            SaleId = createResponse.SaleId.Value
        });

        var updatedItem = updatedSale.Sale!.Items.Single(x => x.ProductId == productA);

        Assert.Equal(originalItemId, updatedItem.Id);
        Assert.Equal(4, updatedItem.Quantity);
        Assert.Equal(40m, updatedItem.Discount);
        Assert.Equal(410m, updatedSale.Sale.TotalAmount);
    }

    private static IServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .AddAutoMapper(typeof(SaleMappingProfile).Assembly)
            .BuildServiceProvider();
    }
}
