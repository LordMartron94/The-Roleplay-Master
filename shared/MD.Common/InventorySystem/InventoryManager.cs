using MD.Common.EconomySystem;

namespace MD.Common.InventorySystem
{
    public class InventoryManager : IInventoryManager
    {
        private readonly List<IItem> _inventoryItems;

        public InventoryManager()
        {
            _inventoryItems = new List<IItem>();
        }

        public void AddToInventory(IItem item)
        {
            if (!_inventoryItems.Contains(item))
                _inventoryItems.Add(item);
        }
        
        public IItem GetItem(string itemId)
        {
            return _inventoryItems.Find(item => item.UuidV4 == itemId);
        }

        public void RemoveFromInventory(IItem item)
        {
            if (_inventoryItems.Contains(item))
                _inventoryItems.Remove(item);
        }

        public void RemoveFromInventory(string itemId)
        {
            IItem item = GetItem(itemId);
            RemoveFromInventory(item);
        }
    }
}