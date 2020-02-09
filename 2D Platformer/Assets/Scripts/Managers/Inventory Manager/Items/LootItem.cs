using Platformer.RNG;
using UnityEngine;

namespace Platformer.Managers.Items
{
    [CreateAssetMenu(fileName = "Loot Item", menuName = "Items/Loot Item", order = 1)]
    public class LootItem : ScriptableObject
    {
        [SerializeField]
        private Item item;
        public Item Item
        {
            get { return item; }
            set { item = value; }
        }

        [SerializeField]
        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }



        [SerializeField]
        [Tooltip("For quantity=2 the 2nd item has Drop%/2 chance, for quantity=3 the 3rd item has Drop%/3 chance, etc.")]
        [Range(0, 1000)]
        private int dropPercentage = 100;
        public int DropPercentage
        {
            get { return dropPercentage; }
            set { dropPercentage = value; }
        }


        public bool HasDropped()
        {
            int droppedQuantity;
            for (droppedQuantity = 0; droppedQuantity < Quantity; droppedQuantity++)
            {
                if (RandomNumberGenerator.NumberBetween(1, 100) >= DropPercentage / droppedQuantity + 1)
                {
                    break;
                }
            }

            Quantity = droppedQuantity;

            if (Quantity > 0)
            {
                return true;
            }

            return false;
        }
    }
}
