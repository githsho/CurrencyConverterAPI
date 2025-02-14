using CurrencyConverterAPI.Models;
using CurrencyConverterAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly ICurrencyConverterService _converterService;

        public CurrencyConverterController(ICurrencyConverterService converterService)
        {
            _converterService = converterService;
        }

        [HttpGet("convert")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Convert(string providerName, string fromCurrency, string toCurrency, decimal amount)
        {
            try
            {
                var convertedAmount = await _converterService.ConvertAsync(providerName, fromCurrency, toCurrency, amount);
                return Ok(convertedAmount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("latestexchangerate")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> LatestExchangeRate(string providerName = "Frankfurter", string baseCurrency = "EUR")
        {

            try
            {
                var exchangeRateResult = await _converterService.GetLatestExchangeRate(providerName, baseCurrency);
                return Ok(exchangeRateResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getratesoveraperiod")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> LatestExchangeRate(string providerName = "Frankfurter", string fromDate = null, string toDate = null)
        {
            if (string.IsNullOrEmpty(fromDate))
            {
                fromDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            try
            {
                var exchangeRateResult = await _converterService.GetRatesOverAPeriodAsync(providerName, fromDate, toDate);
                return Content(exchangeRateResult.ToString(), "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addRate")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRate([FromBody] CurrencyRate rate)
        {
            // Implementation to add currency rate
            return Ok();
        }
    }

}
