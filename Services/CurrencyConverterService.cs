using CurrencyConverterAPI.Models;
using CurrencyConverterAPI.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyConverterAPI.Services
{
    public interface ICurrencyConverterService
    {
        Task<decimal> ConvertAsync(string conversionProvider, string fromCurrency, string toCurrency, decimal amount);
        Task<ExchangeRate> GetLatestExchangeRate(string conversionProvider, string baseCurrency);
        Task<JObject> GetRatesOverAPeriodAsync(string conversionProvider, string fromDate, string toDate);
    }

    public interface ICurrencyRateService
    {
        Task<CurrencyRate> GetRateAsync(string fromCurrency, string toCurrency);
    }

    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly CurrencyRateCache _rateCache;

        public CurrencyConverterService(CurrencyRateCache rateCache)
        {
            _rateCache = rateCache;
        }
        //"Frankfurter"
        public async Task<decimal> ConvertAsync(string conversionProvider, string fromCurrency, string toCurrency, decimal amount)
        {
            var rate = await _rateCache.GetRateAsync(conversionProvider, fromCurrency, toCurrency);
            if (rate == null)
            {
                throw new Exception("Conversion rate not found.");
            }
            return amount * rate.Rate;
        }

        public async Task<ExchangeRate> GetLatestExchangeRate(string conversionProvider, string baseCurrency)
        {
            var exchangerate = await _rateCache.GetLatestExchangeRateAsync(conversionProvider, baseCurrency);
            if (exchangerate == null)
            {
                throw new Exception("Conversion rate not found.");
            }
            return exchangerate;
        }

        public async Task<JObject> GetRatesOverAPeriodAsync(string conversionProvider, string fromDate, string toDate)
        {
            var exchangerate = await _rateCache.GetRatesOverAPeriodAsync(conversionProvider, fromDate, toDate);
            if (exchangerate == null)
            {
                throw new Exception("Conversion rate not found.");
            }
            return exchangerate;
        }
    }

    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly HttpClient _httpClient;

        public CurrencyRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyRate> GetRateAsync(string fromCurrency, string toCurrency)
        {
            var response = await _httpClient.GetAsync($"https://api.frankfurter.app/latest?from={fromCurrency}&to={toCurrency}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var rate = JsonConvert.DeserializeObject<CurrencyRate>(content);
            return rate;
        }
    }

}
