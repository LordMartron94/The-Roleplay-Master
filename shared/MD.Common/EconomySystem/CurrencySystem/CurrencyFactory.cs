namespace MD.Common.EconomySystem.CurrencySystem
{
    public class CurrencyFactory
    {
        public Currency Create(string currencyName, string currencyIconPath = null, string currencyIconUrl = null)
        {
            Currency currency = new Currency(currencyName, currencyIconPath: currencyIconPath, currencyIconUrl: currencyIconUrl);

            return currency;
        }
    }
}