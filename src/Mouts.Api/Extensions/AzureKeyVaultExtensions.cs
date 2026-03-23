using Azure.Identity;

namespace Mouts.Api.Extensions;

public static class AzureKeyVaultExtensions
{
    public static ConfigurationManager AddAzureKeyVault(this ConfigurationManager configuration)
    {
        var keyVaultUri = configuration["KeyVault:VaultUri"];

        if (string.IsNullOrWhiteSpace(keyVaultUri))
            return configuration;

        configuration.AddAzureKeyVault(
            new Uri(keyVaultUri),
            new DefaultAzureCredential()
        );

        return configuration;
    }
}