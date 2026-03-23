namespace Mouts.Domain.Exceptions;

public class SaleDomainException : InvalidOperationException
{
    public SaleDomainException(string message) : base(message)
    {
    }
}
