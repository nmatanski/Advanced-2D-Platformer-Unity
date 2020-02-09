using UnityEngine;

namespace Platformer.Managers.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Healing Item", order = 1)]
    public class HealingItem : Item
    {
        [SerializeField]
        private ushort healAmount;
        public ushort HealAmount
        {
            get { return healAmount; }
            set { healAmount = value; }
        }


        public override void Use(PlayerManager player = null)
        {
            if (player == null)
            {
                Debug.LogWarning("Player is null. You can't heal it.");
                return;
            }

            if (!IsUsable)
            {
                Debug.LogWarning("This healing item is set to not usable (isUsable=false). No healing effects have been applied to the player.");
                return;
            }

            player.AddHealth((short)healAmount);

            Debug.Log($"Player has been healed for {HealAmount}.");
        }
    }
}
