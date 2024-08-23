using System.Diagnostics.CodeAnalysis;

namespace MD.Common.EconomySystem.CurrencySystem.CurrencyConversion
{
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    public class CurrencyConverter
    {
        private Currency _baseCurrency;
        
        private ExchangeRateManager _exchangeRateManager;
        private IExchangeRateCalculator _calculator;

        public CurrencyConverter(Currency baseCurrency, IExchangeRateCalculator calculator = null)
        {
            calculator ??= new DefaultExchangeRateCalculator();
            _calculator = calculator;

            _baseCurrency = baseCurrency;

            _exchangeRateManager = new ExchangeRateManager(_calculator);
            _exchangeRateManager.AddCurrency(_baseCurrency, 1);
        }

        /// <summary>
        /// Expresses one currency in terms of another.
        /// </summary>
        public decimal ConvertCurrency((Currency, decimal) originalCurrency, Currency targetCurrency)
        {
            decimal exchangeRate = _exchangeRateManager.GetFinalExchangeRate(originalCurrency.Item1, targetCurrency);

            return originalCurrency.Item2 * exchangeRate;
        }
        
        /// <summary>
        /// Gets the currency amount in terms of base currency (non-existent).
        /// </summary>
        public decimal ConvertCurrency((Currency, decimal) originalCurrency)
        {
            decimal exchangeRate = _exchangeRateManager.GetFinalExchangeRate(originalCurrency.Item1, _baseCurrency);

            return originalCurrency.Item2 * exchangeRate;
        }

        /// <summary>
        /// Adds a currency to the container of exchange rates if it's not already in there.
        /// </summary>
        /// <param name="baseExchangeRate">The base exchange rate value of the currency.
        /// <br></br>Uses a quotient to signify. i.e., 1 is the exact basic.
        /// <br></br>1.2 means this currency has 20% higher worth than the base.
        /// <br></br> <i>Note: The base currency does not have to be added. It automatically makes use of 1 as a base.</i></param>
        public void AddCurrency(Currency currency, decimal baseRate)
        {
            _exchangeRateManager.AddCurrency(currency, baseRate);
        }

        /// <summary>
        /// Removes a currency if it is existent in the container of exchange rates.
        /// </summary>
        public void RemoveCurrency(Currency currency)
        {
            _exchangeRateManager.RemoveCurrency(currency);
        }

        /// <summary>
        /// Sets the base exchange rate for a currency.
        /// <br></br>Note that the base exchange rate is an exchange rate that doesn't take into account external factors yet.
        /// </summary>
        /// <exception cref="ArgumentException">Throws an exception if the currency can't be found in the container of exchange rates.</exception>
        public void SetBaseExchangeRate(Currency currency, decimal baseRate)
        {
            _exchangeRateManager.SetBaseExchangeRate(currency, baseRate);
        }

        /// <summary>
        /// Adds a factor that influences the worth of a currency. Can be used to create dynamic worths.
        /// </summary>
        /// <param name="weight">The weight for the influencer. The higher this value, the more it will count toward the final value of the exchange rate.</param>
        public void AddWorthInfluencer(IValueFactor valueFactor, int weight)
        {
            _calculator.AddWorthInfluencer(valueFactor, weight);
        }
    }
}