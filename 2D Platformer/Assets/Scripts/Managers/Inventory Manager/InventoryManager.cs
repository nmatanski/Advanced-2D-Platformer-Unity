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
        private int inventoryCapacity = 4;

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


        public bool CollectItem(Item item, int quantity = 1) // to backpack
        {
            ///TODO: check if enough backpack slots and return false

            var backpackInventoryItem = new InventoryItem(item, quantity);
            var backpackItem = Backpack.SingleOrDefault(i => i.Key.ItemDetails.Name.Equals(item.Name)).Key;
            var itemToAdd = backpackInventoryItem;
            bool hasFreeSlotInBackpack = HasFreeSlotInBackpack();

            if (backpackItem == null && !hasFreeSlotInBackpack)
            {
                Debug.Log("Backpack is full."); ///TODO: UI popup
                return false;
            }
            else if (backpackItem != null)
            {
                if (!hasFreeSlotInBackpack && Backpack[backpackItem] % backpackItem.ItemDetails.MaxStackedCount == 0)
                {
                    Debug.Log("Backpack is full."); ///TODO: UI popup
                    return false;
                }

                Backpack[backpackItem] += quantity;
                itemToAdd = new InventoryItem(backpackItem.ItemDetails, Backpack[backpackItem]);

                Backpack.Remove(backpackItem);
            }

            AddItemToBackpack(itemToAdd);

            DisplayItems();

            return true;
        }

        public bool DiscardItem(Item item, int quantity = 1) // from backpack or inventory
        {
            var backpackItem = Backpack.SingleOrDefault(i => i.Key.ItemDetails.Name.Equals(item.Name)).Key;
            var inventoryItem = Inventory.SingleOrDefault(i => i.ItemDetails.Name.Equals(item.Name));

            if (backpackItem == null && inventoryItem != null)
            {
                return UnequipItem(inventoryItem);
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

            DisplayItems();

            return true;
        }

        public bool EquipItem(InventoryItem item) ///TODO: equip on a slot type - helmet, charm, trinket, chest, weapon, shield, etc.
        {
            bool isInInventory = Inventory.SingleOrDefault(i => i.ItemDetails.Name.Equals(item.ItemDetails.Name)) != null;

            if (isInInventory)
            {
                Debug.LogWarning($"The item '{item.ItemDetails.Name}' is already equipped (in the inventory).");
                return false;
            }

            var backpackItem = Backpack.SingleOrDefault(kvp => kvp.Key.ItemDetails.Name.Equals(item.ItemDetails.Name)).Key;
            bool isInBackpack = backpackItem != null;

            if (!isInBackpack)
            {
                Debug.LogWarning($"The item '{item.ItemDetails.Name}' is not in the backpack or inventory. The player has no such item.");
                return false;
            }

            Backpack[backpackItem] = Mathf.Clamp(Backpack[backpackItem] - 1, 0, backpackItem.ItemDetails.MaxStackedCount);

            backpackItem.IsEquipped = false;

            Inventory.Add(new InventoryItem(backpackItem.ItemDetails, 1, true));

            if (Backpack[backpackItem] < 1)
            {
                Backpack.Remove(backpackItem);
            }

            DisplayItems();

            return true;
        }

        public bool UnequipItem(InventoryItem item)
        {
            ///TODO: check if enough backpack slots

            var inventoryItem = Inventory.SingleOrDefault(i => i.ItemDetails.Name.Equals(item.ItemDetails.Name));
            bool isInInventory = Inventory.SingleOrDefault(i => i.ItemDetails.Name.Equals(item.ItemDetails.Name)) != null;

            if (!isInInventory)
            {
                Debug.LogWarning($"The item '{item.ItemDetails.Name}' is not equipped (not in the inventory).");
                return false;
            }

            var backpackItem = Backpack.SingleOrDefault(kvp => kvp.Key.ItemDetails.Name.Equals(item.ItemDetails.Name));
            bool isInBackpack = backpackItem.Key != null;

            if (isInBackpack)
            {
                Backpack[backpackItem.Key]++;
                if (Backpack[backpackItem.Key] > backpackItem.Key.ItemDetails.MaxStackedCount)
                {
                    //backpackItem.Key.IsOverflowed = true;
                }
            }
            else
            {
                Backpack.Add(inventoryItem, 1);
            }

            Inventory.Remove(inventoryItem);

            DisplayItems();

            return true;
        }

        private void AddItemToBackpack(InventoryItem inventoryItem, Dictionary<InventoryItem, int> backpack = null)
        {
            if (backpack == null)
            {
                backpack = Backpack;
            }

            int requiredSlots = RequiredSlots(inventoryItem);
            int emptySlots = EmptySlots();
            int tempQuantity = inventoryItem.Quantity;

            if (requiredSlots > emptySlots)
            {
                tempQuantity = emptySlots * inventoryItem.ItemDetails.MaxStackedCount;
                Debug.Log($"{inventoryItem.Quantity - emptySlots * inventoryItem.ItemDetails.MaxStackedCount} has been left on the ground due to full backpack"); ///TODO: reduce lootItem quantity here
            }

            backpack.Add(inventoryItem, tempQuantity);
        }

        private bool HasFreeSlotInBackpack(Dictionary<InventoryItem, int> backpack = null)
        {
            if (backpack == null)
            {
                backpack = Backpack;
            }

            bool hasOverflowedItem = backpack.FirstOrDefault(kvp => kvp.Key.IsOverflowed).Key != null;

            if (hasOverflowedItem)
            {
                int slotsRequired = 0;

                foreach (var item in backpack)
                {
                    slotsRequired += RequiredSlots(item.Key);
                }

                return inventoryCapacity > slotsRequired;
            }
            else if (inventoryCapacity - backpack.Count < 0)
            {
                Debug.LogWarning($"\tWARNING: Backpack has MORE items than Backpack's Capacity ({backpack.Count} items in {inventoryCapacity} slots");
                return false;
            }
            else
            {
                return inventoryCapacity - backpack.Count > 0;
            }
        }

        private int RequiredSlots(InventoryItem item)
        {
            if (item.Overflow < 1)
            {
                return 1;
            }

            int bonusSlots = Mathf.CeilToInt((float)item.Overflow / item.ItemDetails.MaxStackedCount);
            return bonusSlots + 1;
        }

        private int EmptySlots()
        {
            int slots = 0;
            foreach (var item in Backpack)
            {
                slots += RequiredSlots(item.Key);
            }

            if (inventoryCapacity < slots)
            {
                Debug.LogWarning($"\tWARNING: Backpack has MORE items than Backpack's Capacity ({slots} items in {inventoryCapacity} slots");
            }

            return inventoryCapacity - slots;
        }

        private void DisplayItems()
        {
            string inventoryString = "Backpack:\t\t";

            foreach (var item in Backpack)
            {
                inventoryString += $"{item.Value}x {item.Key.ItemDetails.Name},\t";
            }

            inventoryString += $"\tInventory (equipped items):\t\t";

            foreach (var item in Inventory)
            {
                inventoryString += $"{item.ItemDetails.Name},\t";
            }

            Debug.Log(inventoryString);
        }


        public IEnumerator LoadAndCache()
        {
            yield return StartCoroutine(Load());

            Status = ManagerStatus.Started;
            Debug.Log(Status);

            DisplayItems();
        }

        public IEnumerator Load()
        {
            if (items == null || items.Count < 1)
            {
                ///TODO: try load List of InventoryItem from savefile
            }

            if (items != null)
            {
                foreach (var item in items)
                {
#pragma warning disable S1656 // It has validation in the set method but from the editor bypasses the set method (accessing the serialized private field)
                    item.IsEquipped = item.IsEquipped;
#pragma warning restore S1656 // It has validation in the set method but from the editor bypasses the set method (accessing the serialized private field)

                    CollectItem(item.ItemDetails, item.Quantity);
                    if (item.IsEquipped)
                    {
                        EquipItem(item); ///TODO: Add the item on specific inventory slot - helmet, chest, trinket, charm, etc.
                    }
                }
            }

            yield return null;
        }
    }
}
