using CurrencyConverterAPI.Models;
using CurrencyConverterAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace CurrencyConverterAPI.Utilities
{
    public class CurrencyRateCache
    {
        private readonly IMemoryCache _cache;
        private readonly ICurrencyProviderFactory _providerFactory;

        public CurrencyRateCache(IMemoryCache cache, ICurrencyProviderFactory providerFactory)
        {
            _cache = cache;
            _providerFactory = providerFactory;
        }

        public async Task<CurrencyRate> GetRateAsync(string providerName, string fromCurrency, string toCurrency)
        {
            string cacheKey = $"{providerName}-{fromCurrency}-{toCurrency}";
            if (!_cache.TryGetValue(cacheKey, out CurrencyRate rate))
            {
                var provider = _providerFactory.GetProvider(providerName);
                rate = await provider.GetRateAsync(fromCurrency, toCurrency);
                if (rate != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
                    _cache.Set(cacheKey, rate, cacheEntryOptions);
                }
            }
            return rate;
        }

        public async Task<ExchangeRate> GetLatestExchangeRateAsync(string providerName, string baseCurrency)
        {
            //string paramBaseCurrency = string.IsNullOrEmpty(baseCurrency) ? "EUR" : baseCurrency;
            string cacheKey = $"{providerName}-{baseCurrency}";
            if (!_cache.TryGetValue(cacheKey, out ExchangeRate exchangerate))
            {
                var provider = _providerFactory.GetProvider(providerName);
                exchangerate = await provider.GetLatestExchangeRateAsync(baseCurrency);
                if (exchangerate != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
                    _cache.Set(cacheKey, exchangerate, cacheEntryOptions);
                }
            }
            return exchangerate;
        }

        public async Task<JObject> GetRatesOverAPeriodAsync(string providerName, string fromDate, string toDate)
        {
            //string paramBaseCurrency = string.IsNullOrEmpty(baseCurrency) ? "EUR" : baseCurrency;
            string cacheKey = $"{fromDate}-{toDate}";
            if (!_cache.TryGetValue(cacheKey, out JObject exchangerate))
            {
                var provider = _providerFactory.GetProvider(providerName);
                exchangerate = await provider.GetRatesOverAPeriodAsync(fromDate, toDate);
                if (exchangerate != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
                    _cache.Set(cacheKey, exchangerate, cacheEntryOptions);
                }
            }
            return exchangerate;
        }
    }

}
