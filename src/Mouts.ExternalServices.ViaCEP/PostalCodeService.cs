using System.Net.Http.Json;
using Mouts.Application.ExternalServices.PostalCode;
using Mouts.ExternalServices.ViaCEP.Responses;

namespace Mouts.ExternalServices.ViaCEP;
public class PostalCodeService : IPostalCodeService
{
    private readonly HttpClient _http;

    public PostalCodeService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PostalCodeResult?> GetByCodeAsync(string postalCode)
    {
        var response = await _http.GetFromJsonAsync<ViaCepResponse>($"{postalCode}/json");

        if (response is null)
            return null;

        return new PostalCodeResult
        {
            Street = response.Logradouro ?? string.Empty,
            City = response.Localidade ?? string.Empty,
            State = response.Estado ?? string.Empty
        };
    }
}
