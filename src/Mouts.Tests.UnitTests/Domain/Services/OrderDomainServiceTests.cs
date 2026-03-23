using Mouts.Domain.Services;


namespace Mouts.Tests.UnitTests.Domain.Services;
public class OrderDomainServiceTests
{
    [Fact]
    public void ApplyDiscount_ShouldReduceTotalAmount()
    {
        var service = new OrderDomainService();
        var finalAmount = service.ApplyDiscount(20);
        Assert.Equal(18, finalAmount);
    }
}