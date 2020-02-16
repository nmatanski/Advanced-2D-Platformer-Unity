using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Managers.Items
{
    public class Loot : MonoBehaviour
    {
        [SerializeField]
        private List<LootItem> lootTable;

        private InventoryManager inventory;


        private void Start()
        {
            inventory = InventoryManager.Instance;
            lootTable = new List<LootItem>();

            foreach (var lootItem in lootTable)
            {
                bool hasDropped = lootItem.HasDropped();

                Debug.Log($"Item {lootItem.Item.Name} dropped ({hasDropped}) {lootItem.Quantity} times");

                if (!hasDropped)
                {
                    lootTable.Remove(lootItem);
                }
            }
        }

        public void CollectLoot()
        {
            foreach (var lootItem in lootTable)
            {
                inventory.CollectItem(lootItem.Item, lootItem.Quantity);
                Debug.Log($"Try CollectItem({lootItem.Item.Name})");
            }

            Destroy(gameObject);
        }
    }
}
