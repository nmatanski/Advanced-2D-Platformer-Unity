﻿using UnityEngine;

namespace Platformer.Managers.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 1)]
    public class Item : ScriptableObject
    {
        [SerializeField]
        private new string name;
        public string Name { get => name; set => name = value; }

        [SerializeField]
        private string namePlural;
        public string NamePlural { get => namePlural; set => namePlural = value; }

        [SerializeField]
        private string description;
        public string Description { get => description; set => description = value; }

        [SerializeField]
        private int maxStackedCount;
        public int MaxStackedCount { get => maxStackedCount; set => maxStackedCount = value; }

        [SerializeField]
        private bool isEquippable = false;
        public bool IsEquippable { get => isEquippable; set => isEquippable = value; }

        [SerializeField]
        private bool isUsable = false;
        public bool IsUsable { get => isUsable; set => isUsable = value; }


        public virtual void Use(PlayerManager player = null)
        {
            if (!IsUsable)
            {
                Debug.LogWarning("This item is set to not usable (isUsable=false). No effects have been applied.");
                return;
            }

            Debug.Log("You can't use the default Item type. No effects applied.");
        }
    }
}