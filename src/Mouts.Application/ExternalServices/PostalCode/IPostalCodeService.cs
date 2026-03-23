namespace Mouts.Application.ExternalServices.PostalCode;
public interface IPostalCodeService
{
    Task<PostalCodeResult?> GetByCodeAsync(string postalCode);
}