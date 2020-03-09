using UnityEngine;

namespace Platformer.Managers
{
    public interface ICharacterManager
    {
        Animator CharacterAnimator { get; }
        GameObject DeathEffect { get; }
        int Gold { get; }
        int Health { get; set; }
        bool IsDead { get; }
        bool IsInvulnerable { get; }
        int MaxHealth { get; }

        void AddHealth(short value);
    }
}