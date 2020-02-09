using Platformer.Managers.Items;
using System;
using UnityEngine;

namespace Platformer.Managers
{
    [Serializable]
    public class InventoryItem
    {
        [SerializeField]
        private Item itemDetails;
        public Item ItemDetails { get => itemDetails; set => itemDetails = value; }

        [SerializeField]
        [Range(0, 200)]
        private int quantity;
        public int Quantity { get => quantity; set => quantity = value; }

        [SerializeField]
        private bool isEquipped;
        public bool IsEquipped
        {
            get { return isEquipped; }
            set { isEquipped = value && ItemDetails.IsEquippable; }
        }

        public bool IsOverflowed { get { return Quantity > ItemDetails.MaxStackedCount; } }

        public int Overflow { get { return Quantity - ItemDetails.MaxStackedCount; } }


        public InventoryItem(Item item, int quantity, bool isEquipped = false)
        {
            ItemDetails = item;
            Quantity = quantity;
            IsEquipped = isEquipped;
        }
    }
}