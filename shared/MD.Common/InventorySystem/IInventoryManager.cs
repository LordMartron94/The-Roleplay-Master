using MD.Common.EconomySystem;

namespace MD.Common.InventorySystem
{
    public interface IInventoryManager
    {
        void AddToInventory(IItem item);

        IItem GetItem(string itemId);

        void RemoveFromInventory(IItem item);

        void RemoveFromInventory(string itemId);
    }
}