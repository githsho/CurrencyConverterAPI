using CurrencyConverterAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace CurrencyConverterAPI.Services
{
    public class FrankfurterCurrencyProvider : ICurrencyProvider
    {
        private readonly HttpClient _httpClient;

        public FrankfurterCurrencyProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyRate> GetRateAsync(string fromCurrency, string toCurrency)
        {
            HttpResponseMessage responseMsg;
            CurrencyRate currencyRate = new CurrencyRate();

            var url = "https://api.frankfurter.dev/v1/" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "?base=" + fromCurrency + "&symbols=" + toCurrency;            
            var response = await GetRequest(url);
            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                url = "https://api.frankfurter.dev/v1/" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "?base=" + fromCurrency + "&symbols=" + toCurrency;
                response = await GetRequest(url);
            }

            var content = await response.Content.ReadAsStringAsync();
            var responseObj = JObject.Parse(content);
            currencyRate.Rate = responseObj["rates"][toCurrency].Value<decimal>();
            currencyRate.FromCurrency = fromCurrency;
            currencyRate.ToCurrency = toCurrency;
            //var rate = JsonConvert.DeserializeObject<CurrencyRate>(content);
            return currencyRate;
        }

        public async Task<ExchangeRate> GetLatestExchangeRateAsync(string baseCurrency)
        {
            HttpResponseMessage responseMsg;
            var url = string.Empty;
            ExchangeRate exchangeRate = new ExchangeRate();
            Rates rates = new Rates();
            if (!string.IsNullOrEmpty(baseCurrency))
            {
                url = $"https://api.frankfurter.dev/v1/latest?base={baseCurrency}";
            }
            var response = await GetRequest(url);

            var content = await response.Content.ReadAsStringAsync();
            var responseObj = JObject.Parse(content);
            rates.AUD = responseObj["rates"]["AUD"].Value<decimal>();
            rates.BGN = responseObj["rates"]["BGN"].Value<decimal>();
            rates.BRL = responseObj["rates"]["BRL"].Value<decimal>();
            rates.CAD = responseObj["rates"]["CAD"].Value<decimal>();
            rates.CHF = responseObj["rates"]["CHF"].Value<decimal>();
            rates.CNY = responseObj["rates"]["CNY"].Value<decimal>();
            rates.CZK = responseObj["rates"]["CZK"].Value<decimal>();
            rates.DKK = responseObj["rates"]["DKK"].Value<decimal>();
            rates.GBP = responseObj["rates"]["GBP"].Value<decimal>();
            rates.HKD = responseObj["rates"]["HKD"].Value<decimal>();
            rates.HUF = responseObj["rates"]["HUF"].Value<decimal>();
            rates.IDR = responseObj["rates"]["IDR"].Value<decimal>();
            rates.ILS = responseObj["rates"]["ILS"].Value<decimal>();
            rates.INR = responseObj["rates"]["INR"].Value<decimal>();
            rates.ISK = responseObj["rates"]["ISK"].Value<decimal>();
            rates.JPY = responseObj["rates"]["JPY"].Value<decimal>();
            rates.KRW = responseObj["rates"]["KRW"].Value<decimal>();
            rates.MXN = responseObj["rates"]["MXN"].Value<decimal>();
            rates.MYR = responseObj["rates"]["MYR"].Value<decimal>();
            rates.NOK = responseObj["rates"]["NOK"].Value<decimal>();
            rates.NZD = responseObj["rates"]["NZD"].Value<decimal>();

            rates.PHP = responseObj["rates"]["PHP"].Value<decimal>();
            rates.PLN = responseObj["rates"]["PLN"].Value<decimal>();
            rates.RON = responseObj["rates"]["RON"].Value<decimal>();
            rates.SEK = responseObj["rates"]["SEK"].Value<decimal>();
            rates.SGD = responseObj["rates"]["SGD"].Value<decimal>();
            rates.THB = responseObj["rates"]["THB"].Value<decimal>();

            rates.TRY = responseObj["rates"]["TRY"].Value<decimal>();
            rates.USD = responseObj["rates"]["USD"].Value<decimal>();
            rates.ZAR = responseObj["rates"]["ZAR"].Value<decimal>();

            exchangeRate.amount = responseObj["amount"].Value<decimal>();
            exchangeRate.@base = responseObj["base"].Value<string>();
            exchangeRate.date = responseObj["date"].Value<string>();
            exchangeRate.rates = rates;

            return exchangeRate;
        }

        public async Task<JObject> GetRatesOverAPeriodAsync(string fromDate, string toDate)
        {
            JObject responseObj=null;
            var url = string.Empty;
            CurrencyRate currencyRate = new CurrencyRate();

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                url = "https://api.frankfurter.dev/v1/" + fromDate + ".." + toDate;
            }

            if (!string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                url = "https://api.frankfurter.dev/v1/" + fromDate;
            }

            if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                url = "https://api.frankfurter.dev/v1/" + DateTime.Now.ToString("yyyy-MM-dd");
            }

            var response = await GetRequest(url);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                responseObj = JObject.Parse(content);
            }


            //currencyRate.Rate = responseObj["rates"][toCurrency].Value<decimal>();
            //currencyRate.FromCurrency = fromCurrency;
            //currencyRate.ToCurrency = toCurrency;
            //var rate = JsonConvert.DeserializeObject<CurrencyRate>(content);
            return responseObj;
        }

        public async Task<HttpResponseMessage> GetRequest(string url)
        {
            HttpResponseMessage returnResponse;
            var httpClient = new HttpClient();
            try
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.GetAsync(url);
                return response;
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }

}
