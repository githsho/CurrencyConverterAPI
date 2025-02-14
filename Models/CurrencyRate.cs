namespace CurrencyConverterAPI.Models
{
    public class CurrencyRate
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Rate { get; set; }
    }

}
