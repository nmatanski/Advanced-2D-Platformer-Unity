using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer.Managers
{
    public class InventoryManager : MonoBehaviour, IManager
    {
        public ManagerStatus Status { get; private set; }

        public Dictionary<InventoryItem, int> Backpack { get; private set; }

        public List<InventoryItem> Inventory { get; private set; }

        [SerializeField]
        private List<InventoryItem> items;


        public void Startup()
        {
            Debug.Log("Inventory Manager is starting...");

            Backpack = new Dictionary<InventoryItem, int>();
            Inventory = new List<InventoryItem>();

            Status = ManagerStatus.Initializing;

            ///long-runing startups tasks here, set Status to Initializing 

            StartCoroutine(LoadAndCache());

            Debug.Log(Status);
        }

        private void DisplayItems()
        {
            string inventoryString = "Backpack:\n";

            foreach (var item in Backpack)
            {
                inventoryString += $"{item.Value}x {item.Key.ItemDetails.Name},\t";
            }

            inventoryString += "\nInventory (equipped items):\n";

            foreach (var item in Inventory)
            {
                inventoryString += $"{item.ItemDetails.Name},\t";
            }

            Debug.Log(inventoryString);
        }

        private bool CollectItem(Item item, int quantity = 1) // to backpack
        {
            ///TODO: check if enough backpack slots and return false

            var backpackInventoryItem = new InventoryItem(item, quantity, quantity > item.MaxStackedCount);

            var backpackItem = Backpack.SingleOrDefault(i => i.Key.ItemDetails.Name.Equals(item.Name)).Key;

            if (backpackItem == null)
            {
                Backpack.Add(backpackInventoryItem, backpackInventoryItem.Quantity);
            }
            else
            {
                Backpack[backpackItem] += quantity;
                if (Backpack[backpackItem] > backpackItem.ItemDetails.MaxStackedCount)
                {
                    backpackItem.IsOverflowed = true;
                }
            }

            return true;
        }

        private bool DiscardItem(Item item, int quantity = 1) // from backpack or inventory
        {
            var backpackItem = Backpack.SingleOrDefault(i => i.Key.ItemDetails.Name.Equals(item.Name)).Key;
            var inventoryItem = Inventory.SingleOrDefault(i => i.ItemDetails.Name.Equals(item.Name));

            if (backpackItem == null && inventoryItem != null)
            {
                return UnequipItem(item);
            }
            else if (backpackItem == null)
            {
                Debug.LogWarning($"There is no such item to discard either from backpack or from inventory.");
                return false;
            }

            Backpack[backpackItem] = Mathf.Clamp(Backpack[backpackItem] - quantity, 0, backpackItem.ItemDetails.MaxStackedCount);

            if (Backpack[backpackItem] < 1)
            {
                Backpack.Remove(backpackItem);
            }

            return true;
        }

        private bool EquipItem(Item item)
        {
            bool isInInventory = Inventory.SingleOrDefault(i => i.ItemDetails.Name.Equals(item.Name)) != null;

            if (isInInventory)
            {
                Debug.LogWarning($"The item '{item.Name}' is already equipped (in the inventory).");
                return false;
            }

            var backpackItem = Backpack.SingleOrDefault(kvp => kvp.Key.ItemDetails.Name.Equals(item.Name)).Key;
            bool isInBackpack = backpackItem != null;

            if (!isInBackpack)
            {
                Debug.LogWarning($"The item '{item.Name}' is not in the backpack or inventory. The player has no such item.");
                return false;
            }

            Backpack[backpackItem] = Mathf.Clamp(Backpack[backpackItem] - 1, 0, backpackItem.ItemDetails.MaxStackedCount);

            Inventory.Add(backpackItem);

            if (Backpack[backpackItem] < 1)
            {
                Backpack.Remove(backpackItem);
            }

            return true;
        }

        private bool UnequipItem(Item item)
        {
            ///TODO: check if enough backpack slots

            var inventoryItem = Inventory.SingleOrDefault(i => i.ItemDetails.Name.Equals(item.Name));
            bool isInInventory = Inventory.SingleOrDefault(i => i.ItemDetails.Name.Equals(item.Name)) != null;

            if (!isInInventory)
            {
                Debug.LogWarning($"The item '{item.Name}' is not equipped (not in the inventory).");
                return false;
            }

            var backpackItem = Backpack.SingleOrDefault(kvp => kvp.Key.ItemDetails.Name.Equals(item.Name));
            bool isInBackpack = backpackItem.Key != null;

            if (isInBackpack)
            {
                Backpack[backpackItem.Key]++;
                if (Backpack[backpackItem.Key] > backpackItem.Key.ItemDetails.MaxStackedCount)
                {
                    backpackItem.Key.IsOverflowed = true;
                }
            }
            else
            {
                Backpack.Add(inventoryItem, 1);
            }

            Inventory.Remove(inventoryItem);

            return true;
        }


        public IEnumerator LoadAndCache()
        {
            yield return StartCoroutine(Load());

            Status = ManagerStatus.Started;
            Debug.Log(Status);
        }

        public IEnumerator Load()
        {
            if (items == null || items.Count < 1)
            {
                ///TODO: try load List of InventoryItem from savefile
            }

            if (items != null)
            {
                ///TODO: add all items with item.IsEquipped == true to Inventory
                ///TODO: add all items with item.IsEquipped == false to BackPack dictionary with key InventoryItem and value item.Quantity
            }

            yield return null;
        }
    }
}
