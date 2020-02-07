using System;

namespace Platformer.Managers
{
    [Serializable]
    public class InventoryItem
    {
        public Item ItemDetails { get; set; }

        public int Quantity { get; set; }

        public bool IsOverflowed { get; set; } = false;

        public int Overflow { get { return Quantity - ItemDetails.MaxStackedCount; } }


        public InventoryItem(Item itemDetails, int quantity, bool isOverflowed)
        {
            ItemDetails = itemDetails;
            Quantity = quantity;
            IsOverflowed = isOverflowed;
        }
    }
}