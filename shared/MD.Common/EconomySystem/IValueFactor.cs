namespace MD.Common.EconomySystem
{
    public interface IValueFactor
    {
        /// <summary>
        /// Gets the output for this exchange rate factor for this item.
        /// </summary>
        decimal GetFactorOutput(string itemName);

        void RenameItem(string oldName, string newName);
    }
}