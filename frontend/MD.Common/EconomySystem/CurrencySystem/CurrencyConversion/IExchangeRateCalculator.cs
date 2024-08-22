namespace MD.Common.EconomySystem.CurrencySystem.CurrencyConversion
{
    public interface IExchangeRateCalculator
    {
        void AddWorthInfluencer(IValueFactor factor, int weight);

        void RemoveWorthInfluencer(IValueFactor factor);
        
        internal decimal CalculateExchangeRate(Currency originalCurrency, Currency targetCurrency);
    }
}