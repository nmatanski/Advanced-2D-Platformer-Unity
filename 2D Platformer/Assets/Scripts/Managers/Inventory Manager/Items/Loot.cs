using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Managers.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Loot : MonoBehaviour
    {
        public bool IsReady { get; private set; } = false;

        [SerializeField]
        private List<LootItem> lootTable;
        public List<LootItem> LootTable { get => lootTable; set => lootTable = value; }

        private InventoryManager inventory;
        private SpriteRenderer spriteRenderer;


        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            //inventory = InventoryManager.Instance;
            inventory = Managers.Inventory;
            LootTable = new List<LootItem>();
            var tempLootTable = new List<LootItem>();

            foreach (var lootItem in LootTable)
            {
                bool hasDropped = lootItem.HasDropped();

                Debug.Log($"Item {lootItem.Item.Name} dropped ({hasDropped}) {lootItem.Quantity} times");

                if (!hasDropped)
                {
                    tempLootTable.Add(lootItem);
                    //LootTable.Remove(lootItem);
                }
            }

            foreach (var item in tempLootTable)
            {
                LootTable.Remove(item);
            }

            ///TODO: IMPORTANT: Create a collectible object for EVERY LootItem - monobehavior Collectible with the LootItem

            spriteRenderer.sortingLayerName = "Player";
            spriteRenderer.sortingOrder = -30;
            if (!spriteRenderer.sprite)
            {
                spriteRenderer.sprite = LootTable[Random.Range(0, LootTable.Count)].Item.Icon; ///TODO: Test if Icon will be appropriate
            }

            IsReady = true;
        }

        ///TODO: Change this to be in CollectibleObject
        public void CollectLoot() ///TODO: call this in ontriggerenter2d 
        {
            while (true)
            {
                if (IsReady)
                {
                    break;
                }
            }

            var notCollectedLoot = new List<LootItem>();
            foreach (var lootItem in LootTable)
            {
                bool hasCollected = inventory.CollectItem(lootItem.Item, lootItem.Quantity);
                Debug.Log($"Try CollectItem({lootItem.Item.Name})");

                if (!hasCollected)
                {
                    ///TODO: Add UI warning for full backpack and how many items have been left on the ground
                    notCollectedLoot.Add(lootItem);
                }
            }

            if (notCollectedLoot.Count < 1)
            {
                Destroy(gameObject);
            }
            else
            {
                LootTable = new List<LootItem>(notCollectedLoot);
            }
        }
    }
}
