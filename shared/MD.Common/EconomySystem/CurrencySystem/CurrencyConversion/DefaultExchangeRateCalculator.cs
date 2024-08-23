namespace MD.Common.EconomySystem.CurrencySystem.CurrencyConversion
{
    public class DefaultExchangeRateCalculator : IExchangeRateCalculator
    {
        private Dictionary<IValueFactor, int> _worthInfluencers;

        public DefaultExchangeRateCalculator()
        {
            _worthInfluencers = new Dictionary<IValueFactor, int>();
        }

        public void AddWorthInfluencer(IValueFactor factor, int weight)
        {
            if (_worthInfluencers.ContainsKey(factor))
                return;
            
            _worthInfluencers.Add(factor, weight);
        }

        public void RemoveWorthInfluencer(IValueFactor factor)
        {
            if (!_worthInfluencers.ContainsKey(factor))
                return;

            _worthInfluencers.Remove(factor);
        }

        /// <summary>
        /// Calculates the final exchange rate between two currencies by applying external factors that influence the worth of the currencies.
        /// </summary>
        decimal IExchangeRateCalculator.CalculateExchangeRate(Currency originalCurrency, Currency targetCurrency)
        {
            decimal originalWorth = 0;
            decimal targetWorth = 0;

            if (_worthInfluencers.Count <= 0)
                return 0;

            foreach (KeyValuePair<IValueFactor, int> worthInfluencerWeight in _worthInfluencers)
            {
                originalWorth += worthInfluencerWeight.Key.GetFactorOutput(originalCurrency.Name) * worthInfluencerWeight.Value;
                targetWorth += worthInfluencerWeight.Key.GetFactorOutput(targetCurrency.Name) * worthInfluencerWeight.Value;
            }
            
            return targetWorth / originalWorth;
        }
    }
}