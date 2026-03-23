
namespace Mouts.Domain.Services;
public class OrderDomainService
{
    private readonly decimal _discountRate = 0.1m; 

    public decimal ApplyDiscount(decimal totalAmount)
    {
        return totalAmount - (totalAmount * _discountRate);
    }
}