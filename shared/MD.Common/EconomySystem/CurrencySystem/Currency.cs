namespace MD.Common.EconomySystem.CurrencySystem
{
    public struct Currency
    {
        public string Name { get; }
        public string CurrencyIconUrl { get; set; }
        public string CurrencyIconPath { get; set; }

        internal Currency(string name, string currencyIconUrl = null, string currencyIconPath = null)
        {
            Name = name;
            CurrencyIconUrl = currencyIconUrl;
            CurrencyIconPath = currencyIconPath;
        }
    }
}