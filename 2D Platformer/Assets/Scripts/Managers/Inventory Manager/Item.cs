using System;

namespace Platformer.Managers
{
    [Serializable]
    public class Item
    {
        public string Name { get; set; }

        public string NamePlural { get; set; }

        public string Description { get; set; }

        public int MaxStackedCount { get; set; }

        public bool IsEquipped { get; set; }
    }
}