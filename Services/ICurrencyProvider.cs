using CurrencyConverterAPI.Models;
using Newtonsoft.Json.Linq;

namespace CurrencyConverterAPI.Services
{
    public interface ICurrencyProvider
    {
        Task<CurrencyRate> GetRateAsync(string fromCurrency, string toCurrency);
        Task<ExchangeRate> GetLatestExchangeRateAsync(string baseCurrency);
        Task<JObject> GetRatesOverAPeriodAsync(string fromDate, string toDate);
    }

}
