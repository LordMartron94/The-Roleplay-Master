using System.Diagnostics.CodeAnalysis;
using MD.Common.EconomySystem.CurrencySystem;

namespace MD.Common.EconomySystem.ShopSystem
{
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    public class ShopManager
    {
        private readonly List<ShopCategoryData> _shopInventory;

        public ShopManager()
        {
            _shopInventory = new List<ShopCategoryData>();
        }

        /// <summary>
        /// It's a safe add system, if the shop already contains this specific item (same GUID) then nothing will happen. 
        /// </summary>
        /// <param name="maxFactorInfluences">The first value is the max decrease influence.
        /// <br></br>The second value is the max increase factor.
        /// <br></br>Defaults to 0.5;2 meaning it can be maximum half or double the price based on external factors.</param>
        /// <param name="maxFactorInfluences">How much influence the factors can have on the price.
        /// <br></br>Defaults to 0.5;2 meaning it can maximally be half or double the price.
        /// <br></br><i>Only needs to be filled in if it's the first time an item of this name is added to the store.</i></param>
        public void AddItemToStore(IItem item, long itemBaseCost, Currency defaultCurrency, out bool success, (float, float) maxFactorInfluences = default)
        {
            if (maxFactorInfluences == default)
                maxFactorInfluences = (0.5f, 2);
            
            bool itemDataExists = _shopInventory.Any(shopItemData => shopItemData.ItemName == item.Name);

            if (!itemDataExists)
            {
                ShopCategoryData shopCategoryData = new ShopCategoryData(item.Name, itemBaseCost, defaultCurrency, maxFactorInfluences);
                shopCategoryData.AddItem(item);
                item.SubscribeChangeName(RenameItem);
                
                _shopInventory.Add(shopCategoryData);
                
                success = true;
                return;
            }
            
            ShopCategoryData associatedCategoryData = _shopInventory.FirstOrDefault(shopItemData => shopItemData.ItemName == item.Name);

            if (ItemInStore(associatedCategoryData, item))
            {
                success = false;
                return;
            }
            
            associatedCategoryData.AddItem(item);
            
            item.SubscribeChangeName(RenameItem);

            success = true;
        }

        private bool ItemInStore(ShopCategoryData associatedCategoryData, IItem item)
        {
            bool itemInStore = associatedCategoryData.ItemInStore(item);

            return itemInStore;
        }

        /// <summary>
        /// Adds a value factor to an item. Note that it applies to all items with the same name in the store.
        /// </summary>
        /// <param name="valueFactor">The value factor to add.</param>
        /// <param name="weight">The relative weight of the factor.
        /// <br></br>The higher this value, the more it counts towards the final price of the item.</param>
        /// <param name="maxFactorInfluences">How much influence the factors can have on the price.
        /// <br></br>Defaults to 0.5;2 meaning it can maximally be half or double the price.
        /// <br></br><i>Only needs to be filled in if it's the first time an item of this name is added to the store.</i></param>
        public void AddValueFactorToItem(string itemName, IValueFactor valueFactor, int weight, (float, float) maxFactorInfluences = default)
        {
            ShopCategoryData? associatedItemData = _shopInventory.FirstOrDefault(shopItemData => shopItemData.ItemName == itemName);
            
            if (associatedItemData == null)
                throw new ArgumentException(
                    "Trying to add value factor to non-existent item! Add at least one item of this sort to the store first!");
            
            List<IValueFactor> valueFactors = associatedItemData.ValueFactors;
                
            if (valueFactors.Contains(valueFactor))
                return;

            ShopCategoryData associatedItemDataNew = associatedItemData;
                
            associatedItemDataNew.AddValueFactor(valueFactor, weight);
        }

        private void RenameItem(string oldName, string newName, string? itemId = null)
        {
            ShopCategoryData? associatedItemData = _shopInventory.FirstOrDefault(shopItemData => shopItemData.ItemName == oldName);
            
            if (associatedItemData == null)
                throw new ArgumentException(
                    "Trying to rename non-existent item! Add at least one item of this sort to the store first!");

            if (associatedItemData.Renaming != (oldName, newName))
                associatedItemData.Rename(newName, itemId);
        }

        /// <summary>
        /// Removes an item from the store.
        /// </summary>
        public void RemoveItemFromStore(IItem item, out bool success)
        {
            ShopCategoryData? associatedItemData = _shopInventory.FirstOrDefault(shopItemData => shopItemData.ItemName == item.Name);
            
            if (associatedItemData == null)
                throw new ArgumentException(
                    "Trying to remove item from non-existent item category! Add at least one item of this sort to the store first!");
            
            if (!associatedItemData.ItemInStore(item))
            {
                success = false;
                return;
            }

            success = true;
            associatedItemData.RemoveItem(item);
            
            item.UnsubscribeChangeName(RenameItem);
        }

        /// <summary>
        /// Buys an item from the store if the name corresponds to an available item.
        /// </summary>
        /// <param name="boughtItem">Out variable, the item that is bought. Null if failed.</param>
        /// <exception cref="Exception">Raises an exception if the item can't be found.</exception>
        public void BuyItem(Wallet wallet, string itemName, out IItem? boughtItem)
        {
            boughtItem = null;
            
            ShopCategoryData? associatedItemData = _shopInventory.FirstOrDefault(shopItemData => shopItemData.ItemName == itemName);
            
            if (associatedItemData == null)
                throw new ArgumentException(
                    "Trying to add value factor to non-existent item! Add at least one item of this sort to the store first!");

            Currency itemCurrency = wallet.GetCurrency(associatedItemData.DefaultCurrencyUsed.Name);

            // Withdraw currency & purchase item.
            wallet.Withdraw(itemCurrency, associatedItemData.GetFinalPrice().Item1, out bool bought);
            
            if (!bought)
                return;

            IItem item = associatedItemData.GetItem(itemName);

            RemoveItemFromStore(item, out bool _);

            boughtItem = item;
        }

        public (long, decimal) TestFinalPrice(string itemName)
        {
            ShopCategoryData? associatedItemData = _shopInventory.FirstOrDefault(shopItemData => shopItemData.ItemName == itemName);
            
            if (associatedItemData == null)
                throw new ArgumentException(
                    "Trying to add value factor to non-existent item! Add at least one item of this sort to the store first!");
        
            return associatedItemData.GetFinalPrice();
        }
    }
}