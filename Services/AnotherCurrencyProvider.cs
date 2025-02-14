using CurrencyConverterAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyConverterAPI.Services
{
    public class AnotherCurrencyProvider : ICurrencyProvider
    {
        private readonly HttpClient _httpClient;

        public AnotherCurrencyProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyRate> GetRateAsync(string fromCurrency, string toCurrency)
        {
            // Replace with the actual API call
            var response = await _httpClient.GetAsync($"https://api.anotherprovider.com/latest?from={fromCurrency}&to={toCurrency}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var rate = JsonConvert.DeserializeObject<CurrencyRate>(content);
            return rate;
        }

        public async Task<ExchangeRate> GetLatestExchangeRateAsync(string baseCurrency)
        {
            // Replace with the actual API call
            var response = await _httpClient.GetAsync($"https://api.anotherprovider.com/latest");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var rate = JsonConvert.DeserializeObject<ExchangeRate>(content);
            return rate;
        }

        public async Task<JObject> GetRatesOverAPeriodAsync(string fromDate, string toDate)
        {
            // Replace with the actual API call
            var response = await _httpClient.GetAsync($"https://api.anotherprovider.com/latest?from={fromDate}..{toDate}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var rate = JsonConvert.DeserializeObject<JObject>(content);
            return rate;
        }
    }

}
