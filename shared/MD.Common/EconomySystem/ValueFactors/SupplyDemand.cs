namespace MD.Common.EconomySystem.ValueFactors
{
    /// <summary>
    /// A class that represents the influence of supply and demand.
    /// </summary>
    public class SupplyDemand : IValueFactor
    {
        private readonly Dictionary<string, int> _itemSupply;
        private readonly Dictionary<string, int> _itemDemand;

        /// <summary>
        /// Initializes a new instance of the SupplyDemand class.
        /// </summary>
        public SupplyDemand()
        {
            _itemSupply = new Dictionary<string, int>();
            _itemDemand = new Dictionary<string, int>();
        }

        /// <summary>
        /// Gets the factor output for the specified item.
        /// </summary>
        /// <param name="itemName">The currency to get the factor output for.</param>
        /// <returns>The factor output for the specified item.</returns>
        /// <exception cref="ArgumentException">Thrown if the specified currency has not yet been added to the SupplyDemand object.</exception>
        public decimal GetFactorOutput(string itemName)
        {
            if (!_itemSupply.ContainsKey(itemName) || !_itemDemand.ContainsKey(itemName))
                throw new ArgumentException("The specified item has not (yet) been added to the SupplyDemand object.");

            return (decimal)_itemDemand[itemName] / _itemSupply[itemName];
        }

        /// <summary>
        /// Adds the specified item to the SupplyDemand with the given supply and demand values.
        /// </summary>
        /// <param name="itemName">The item to add to the SupplyDemand.</param>
        /// <param name="supply">The supply value for the item.</param>
        /// <param name="demand">The demand value for the item.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the supply or demand values are less than or equal to zero.</exception>
        public void AddItemToFactor(string itemName, int supply, int demand)
        {
            if (supply <= 0 || demand <= 0)
                throw new ArgumentException("The supply and demand values must be greater than zero.");

            _itemSupply.Add(itemName, supply);
            _itemDemand.Add(itemName, demand);
        }
        
        /// <summary>
        /// Removes the specified item from the SupplyDemandFactor.
        /// </summary>
        /// <param name="itemName">The item to remove from the SupplyDemand.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the specified item is not found in the SupplyDemandFactor.</exception>
        public void RemoveItem(string itemName)
        {
            if (!_itemSupply.ContainsKey(itemName) || !_itemDemand.ContainsKey(itemName))
                throw new KeyNotFoundException("The specified item is not found in the SupplyDemand object.");

            _itemSupply.Remove(itemName);
            _itemDemand.Remove(itemName);
        }

        /// <summary>
        /// Updates the supply and demand values for the specified item.
        /// </summary>
        /// <param name="itemName">The item to update the supply and demand values for.</param>
        /// <param name="supply">The new supply value for the item.</param>
        /// <param name="demand">The new demand value for the item.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the supply or demand values are less than or equal to zero.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the specified item has not yet been added to the SupplyDemand object.</exception>
        public void UpdateItemValues(string itemName, int supply, int demand)
        {
            if (supply <= 0 || demand <= 0)
                throw new ArgumentException("The supply and demand values must be greater than zero.");

            if (!_itemSupply.ContainsKey(itemName) || !_itemDemand.ContainsKey(itemName))
                throw new KeyNotFoundException("The specified item has not yet been added to the SupplyDemand object.");

            _itemSupply[itemName] = supply;
            _itemDemand[itemName] = demand;
        }
        
        /// <summary>
        /// Renames the specified item in the SupplyDemand.
        /// </summary>
        /// <param name="oldItemName">The current name of the item to rename.</param>
        /// <param name="newItemName">The new name for the item.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the specified item is not found in the SupplyDemand.</exception>
        public void RenameItem(string oldItemName, string newItemName)
        {
            if (!_itemSupply.ContainsKey(oldItemName) || !_itemDemand.ContainsKey(oldItemName))
                throw new KeyNotFoundException("The specified item is not found in the SupplyDemand object.");

            int supply = _itemSupply[oldItemName];
            int demand = _itemDemand[oldItemName];

            _itemSupply.Remove(oldItemName);
            _itemDemand.Remove(oldItemName);

            _itemSupply.Add(newItemName, supply);
            _itemDemand.Add(newItemName, demand);
        }

    }
}
