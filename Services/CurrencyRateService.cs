using CurrencyConverterAPI.Models;
using Newtonsoft.Json;

namespace CurrencyConverterAPI.Services
{
    public interface ICurrencyRateService
    {
        Task<CurrencyRate> GetRateAsync(string fromCurrency, string toCurrency);
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
            // Parse the JSON response and create a CurrencyRate object
            var rate = JsonConvert.DeserializeObject<CurrencyRate>(content);

            return rate;
        }
    }

}
