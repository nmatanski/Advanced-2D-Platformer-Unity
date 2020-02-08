using UnityEngine;

namespace Platformer.Managers
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
        private bool isEquippable;
        public bool IsEquippable { get => isEquippable; set => isEquippable = value; }
    }
}