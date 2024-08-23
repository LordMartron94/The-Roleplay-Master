using MD.Common.EconomySystem.CurrencySystem;

namespace MD.Common.EconomySystem.ShopSystem
{
    public class ShopCategoryData
    {
        public long BaseCost { get; private set; }
        public Currency DefaultCurrencyUsed { get; }
        
        private readonly Dictionary<IValueFactor, int> _valueFactors;

        private readonly float _maxFactorDecreaseInfluence;
        private readonly float _maxFactorIncreaseInfluence;
        
        public string ItemName { get; private set; }

        private readonly List<IItem> _associatedItemsInStore;

        public List<IValueFactor> ValueFactors => _valueFactors.Keys.ToList();
        
        public (string oldName, string newName) Renaming { get; private set; }

        /// <param name="itemBaseCost">The base cost of the item. The factors of price influencers will use this to calculate the final value.</param>
        /// <param name="defaultCurrencyUsed">The default currency to be used to pay for this item.</param>
        /// <param name="maxFactorInfluences">The first value is the max decrease influence.
        /// <br></br>The second value is the max increase factor.
        /// <br></br>Defaults to 0.5;2 meaning it can be maximum half or double the price based on external factors.</param>
        public ShopCategoryData(string itemName, long itemBaseCost, Currency defaultCurrencyUsed, (float, float) maxFactorInfluences = default)
        {
            ItemName = itemName;
            BaseCost = itemBaseCost;
            DefaultCurrencyUsed = defaultCurrencyUsed;
            
            _associatedItemsInStore = new List<IItem>();
            _valueFactors = new Dictionary<IValueFactor, int>();

            if (maxFactorInfluences == default)
                maxFactorInfluences = (0.5f, 2);
            
            _maxFactorDecreaseInfluence = maxFactorInfluences.Item1;
            _maxFactorIncreaseInfluence = maxFactorInfluences.Item2;

            Renaming = ("", "");
        }

        /// <summary>
        /// Sets the base cost of the item directly.
        /// </summary>
        public void SetBaseCost(long cost) { BaseCost = cost; }
        
        /// <summary>
        /// Alters the cost based on a modifier...
        /// <br></br>Example: New cost = Cost * Modifier
        /// <br></br>New cost = 50 * 0.5
        /// <br></br>New cost = 25.
        /// </summary>
        /// <param name="modifier">A modifier of which the cost will be multiplied against.</param>
        public void AlterBaseCost(double modifier)
        {
            double newCost = BaseCost * modifier;
            BaseCost = Convert.ToInt64(newCost);
        }

        /// <summary>
        /// Calculates the final price based upon the dynamic external price factors as well as the base price.
        /// </summary>
        /// <returns>1: The final price of the item.
        /// <br></br>2: The squished factor.</returns>
        public (long, decimal) GetFinalPrice()
        {
            decimal totalFactorInfluence = CalculateFactorInfluences();
            decimal totalFactorInfluenceSquished = SquishFactorInfluence(totalFactorInfluence);

            return (Convert.ToInt64(BaseCost * totalFactorInfluenceSquished), totalFactorInfluenceSquished);
        }
        
        /// <summary>
        /// Adds a value influencer (factor) to this item.
        /// </summary>
        /// <param name="factor">The value factor calculator.</param>
        /// <param name="weight">The weight associated with the value factor.</param>
        /// <exception cref="ArgumentException">Throws an argument exception if the factor is already in the list.</exception>
        public void AddValueFactor(IValueFactor factor, int weight)
        {
            if (_valueFactors.ContainsKey(factor))
                throw new ArgumentException($"Factor {factor} already in list!");
            
            _valueFactors.Add(factor, weight);
        }
        
        /// <summary>
        /// Removes a value influencer (factor) from this item.
        /// </summary>
        /// <param name="factor">The value factor calculator.</param>
        /// <exception cref="ArgumentException">Throws an argument exception if the factor is not in the list.</exception>
        public void RemoveValueFactor(IValueFactor factor)
        {
            if (!_valueFactors.ContainsKey(factor))
                throw new ArgumentException($"Trying to delete {factor} but it isn't stored!");

            _valueFactors.Remove(factor);
        }
        
        private decimal CalculateFactorInfluences()
        {
            // If we have 0 dynamic factors, we return 1, so that it has zero influence on the final calculation.
            if (_valueFactors.Count <= 0)
                return 1;
            
            decimal factorInfluence = 0;
            decimal totalWeight = 0;

            foreach (KeyValuePair<IValueFactor, int> factorWeight in _valueFactors)
            {
                factorInfluence += factorWeight.Key.GetFactorOutput(ItemName) * factorWeight.Value;
                totalWeight += factorWeight.Value;
            }
            
            return factorInfluence / totalWeight;
        }
        
        private decimal SquishFactorInfluence(decimal totalFactorInfluence)
        {
            decimal squished = Math.Max((decimal)_maxFactorDecreaseInfluence, Math.Min(totalFactorInfluence, (decimal)_maxFactorIncreaseInfluence));

            return squished;
        }
        
        /// <summary>
        /// Checks if an item is in store by checking if any of the items have the same ID.
        /// </summary>
        /// <param name="item">The item to check for.</param>
        public bool ItemInStore(IItem item) { return _associatedItemsInStore.Any(itemInStore => itemInStore.UuidV4 == item.UuidV4); }
        
        public void AddItem(IItem item)
        {
            // Makes sure that the item is not yet in the store by throwing an exception if any of the items has the same ID.
            if (ItemInStore(item))
                throw new Exception("Item already exist in store!");
            
            _associatedItemsInStore.Add(item);
        }

        public void RemoveItem(IItem item)
        {
            // Makes sure that the item is in the store by checking that none of the items has the same ID, if it does have, then throw exception.
            if (!ItemInStore(item))
                throw new Exception("Item not yet existent in store!");
            
            _associatedItemsInStore.Remove(item);
        }

        public IItem GetItem(string itemName)
        {
            return _associatedItemsInStore.FirstOrDefault(item => item.Name == itemName);
        }

        /// <summary>
        /// Renames item (category) and all items of it.
        /// </summary>
        public void Rename(string newName, string? idOfFirstItemChanging = null)
        {
            foreach (IItem item in _associatedItemsInStore)
            {
                if (idOfFirstItemChanging != null && item.UuidV4 == idOfFirstItemChanging)
                {
                    Renaming = (item.Name, newName);
                    continue;
                }

                item.ChangeName(newName);
            }

            foreach (IValueFactor factor in _valueFactors.Keys)
                factor.RenameItem(ItemName, newName);

            ItemName = newName;
        }
    }
}