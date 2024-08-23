using System.Diagnostics.CodeAnalysis;
using MD.Common.EconomySystem.CurrencySystem;

namespace MD.Common.EconomySystem.ShopSystem
{
    /// <summary>
    /// The wallet class.
    /// </summary>
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    public class Wallet
    {
        private Dictionary<Currency, long> _currencyMaxNegatives;
        private Dictionary<Currency, long> _currencyAmounts;

        public Wallet()
        {
            _currencyMaxNegatives = new Dictionary<Currency, long>();
            _currencyAmounts = new Dictionary<Currency, long>();
        }

        /// <summary>
        /// Deposits currency unto the wallet.
        /// </summary>
        public void Deposit(Currency currency, long depositAmount)
        {
            if (!_currencyAmounts.ContainsKey(currency))
                throw new Exception("Trying to deposit into currency not existent in wallet!");

            if (depositAmount < 0)
                throw new Exception("It appears you are trying to deposit a negative amount! Use the withdrawal method instead!");

            _currencyAmounts[currency] += depositAmount;
        }

        /// <summary>
        /// Withdraws currency from wallet.
        /// Will do nothing if the amount goes under the max negative.
        /// </summary>
        public void Withdraw(Currency currency, long withdrawalAmount, out bool success)
        {
            success = false;
            
            if (!_currencyAmounts.ContainsKey(currency))
                throw new Exception("Trying to withdraw from currency not existent in wallet!");
            
            if (withdrawalAmount < 0)
                throw new Exception("It appears you are trying to withdraw a negative amount! Use the deposit method instead!");

            if(!CanWithdraw(currency, withdrawalAmount))
                return;
            
            _currencyAmounts[currency] -= withdrawalAmount;

            success = true;
        }

        /// <summary>
        /// Checks if amount of currency can be withdrawn from this wallet.
        /// </summary>
        private bool CanWithdraw(Currency currency, long withdrawalAmount)
        {
            return _currencyAmounts[currency] - withdrawalAmount > _currencyMaxNegatives[currency];
        }

        /// <summary>
        /// Gets the amount of currency from a certain currency in the wallet.
        /// </summary>
        /// <exception cref="Exception">Raises an exception if the currency is not found in the wallet.</exception>
        public double GetAmount(string currencyName)
        {
            List<string> currencyNames = _currencyAmounts.Keys.Select(currency => currency.Name).ToList();
            
            if (!currencyNames.Contains(currencyName))
                throw new Exception($"Currency: {currencyName} not found!");

            Currency currency = _currencyAmounts.Keys.FirstOrDefault(_currency => _currency.Name == currencyName);

            return _currencyAmounts[currency];
        }

        public Currency GetCurrency(string currencyName) { return _currencyAmounts.Keys.FirstOrDefault(currency => currency.Name == currencyName); }

        /// <summary>
        /// Adds a currency to the list of currencies if it doesn't exist already.
        /// </summary>
        public void AddCurrency(Currency currency, long starterCurrencyAmount = 0, long currencyMaxNegative = 0)
        {
            if (_currencyAmounts.Keys.Any(walletCurrency => walletCurrency.Name == currency.Name))
                return;

            _currencyAmounts.Add(currency, starterCurrencyAmount);
            _currencyMaxNegatives.Add(currency, currencyMaxNegative);
        }

        public List<Currency> GetCurrenciesInWallet() { return _currencyAmounts.Keys.ToList(); }
    }
}