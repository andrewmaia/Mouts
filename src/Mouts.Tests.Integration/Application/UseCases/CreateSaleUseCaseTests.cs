using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mouts.Application.Common.Events;
using Mouts.Application.UseCases.CreateSale;
using Mouts.Application.UseCases.GetSaleById;
using Mouts.Application.UseCases.Common;
using Mouts.Infrastructure.PostgreSQL.Context;
using Mouts.Infrastructure.PostgreSQL.Repositories;
using Mouts.Tests.Integration.Application.Fixtures;

namespace Mouts.Tests.Integration.Application.UseCases;

public class CreateSaleUseCaseTests
{
    [Fact]
    public async Task Should_CreateSaleSuccessfully_InMemory()
    {
        var options = new DbContextOptionsBuilder<MoutsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new MoutsDbContext(options);

        var saleRepository = new SaleRepository(context);
        var unitOfWork = new UnitOfWorkInMemory(context);
        var dispatcher = new DomainEventsDispatcher(BuildServiceProvider());

        var createUseCase = new CreateSaleUseCase(saleRepository, unitOfWork, dispatcher);
        var getByIdUseCase = new GetSaleByIdUseCase(saleRepository);

        var createResponse = await createUseCase.ExecuteAsync(new CreateSaleRequest
        {
            SaleNumber = "S-1000",
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = "Customer A",
            BranchId = Guid.NewGuid(),
            BranchName = "Branch A",
            Items =
            [
                new SaleItemInput
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Notebook",
                    Quantity = 10,
                    UnitPrice = 100m
                }
            ]
        });

        Assert.True(createResponse.IsSuccess);
        Assert.NotNull(createResponse.SaleId);

        var getResponse = await getByIdUseCase.ExecuteAsync(new GetSaleByIdRequest
        {
            SaleId = createResponse.SaleId!.Value
        });

        Assert.True(getResponse.IsSuccess);
        Assert.NotNull(getResponse.Sale);
        Assert.Equal(800m, getResponse.Sale!.TotalAmount);
        Assert.Single(getResponse.Sale.Items);
        Assert.Equal(200m, getResponse.Sale.Items.Single().Discount);
    }

    private static IServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection().BuildServiceProvider();
    }
}
