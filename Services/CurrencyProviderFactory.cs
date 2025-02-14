namespace CurrencyConverterAPI.Services
{
    public interface ICurrencyProviderFactory
    {
        ICurrencyProvider GetProvider(string providerName);
    }

    public class CurrencyProviderFactory : ICurrencyProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CurrencyProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICurrencyProvider GetProvider(string providerName)
        {
            return providerName switch
            {
                "Frankfurter" => _serviceProvider.GetService<FrankfurterCurrencyProvider>(),
                "AnotherProvider" => _serviceProvider.GetService<AnotherCurrencyProvider>(),
                _ => throw new ArgumentException("Invalid provider name", nameof(providerName)),
            };
        }
    }

}
