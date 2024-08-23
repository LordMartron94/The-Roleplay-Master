using System.Diagnostics.CodeAnalysis;

namespace MD.Common.EconomySystem.CurrencySystem.CurrencyConversion
{
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    internal class ExchangeRateManager
    {
        private Dictionary<Currency, decimal> _exchangeRates;
        
        private IExchangeRateCalculator _exchangeRateCalculator;

        public ExchangeRateManager(IExchangeRateCalculator exchangeRateCalculator)
        {
            _exchangeRates = new Dictionary<Currency, decimal>();
            _exchangeRateCalculator = exchangeRateCalculator;
        }
        
        public void AddCurrency(Currency currency, decimal baseExchangeRate)
        {
            if (_exchangeRates.ContainsKey(currency))
                return;
            
            _exchangeRates.Add(currency, baseExchangeRate);
        }
        
        public void RemoveCurrency(Currency currency)
        {
            if (!_exchangeRates.ContainsKey(currency))
                return;

            _exchangeRates.Remove(currency);
        }

        public void SetBaseExchangeRate(Currency currency, decimal exchangeRate)
        {
            if (!_exchangeRates.ContainsKey(currency))
                throw new ArgumentException($"Currency: {currency.Name} not existent in container of exchange rates!\nMake sure to add it first!");

            _exchangeRates[currency] = exchangeRate;
        }

        /// <exception cref="ArgumentException">Throws an exception if the currency can't be found in the container of exchange rates.</exception>
        public decimal GetBaseExchangeRate(Currency currency)
        {
            if (_exchangeRates.ContainsKey(currency))
                return _exchangeRates[currency];

            throw new ArgumentException($"Currency: {currency.Name} not existent in container of exchange rates!\nMake sure to add it first!");
        }

        /// <exception cref="ArgumentException">Throws an exception if one of the currencies can't be found in the container of exchange rates.</exception>
        public decimal GetFinalExchangeRate(Currency currency, Currency targetCurrency)
        {
            if (!_exchangeRates.ContainsKey(currency) || !_exchangeRates.ContainsKey(targetCurrency))
                throw new ArgumentException($"Currency: {currency.Name} or {targetCurrency.Name} not existent in container of exchange rates!\nMake sure to add it first!");

            decimal originalBaseRate = GetBaseExchangeRate(currency);
            decimal targetBaseRate = GetBaseExchangeRate(targetCurrency);
            decimal baseRate = originalBaseRate / targetBaseRate;

            decimal additionalRate = _exchangeRateCalculator.CalculateExchangeRate(currency, targetCurrency);

            return baseRate + additionalRate;
        }
    }
}