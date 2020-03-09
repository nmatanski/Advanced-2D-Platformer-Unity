using Platformer.Managers;
using UnityEngine;

namespace Platformer.AI
{
    public class AIManager : MonoBehaviour, ICharacterManager
    {
        public Animator CharacterAnimator { get; private set; }

        [SerializeField]
        private int health = 100;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        [SerializeField]
        private int maxHealth = 100;
        public int MaxHealth
        {
            get { return maxHealth; }
            private set { maxHealth = value; }
        }

        [SerializeField]
        private int gold;
        public int Gold
        {
            get { return gold; }
            private set { gold = value; }
        }

        [SerializeField]
        private bool isDead = false;
        public bool IsDead
        {
            get { return isDead; }
            private set { isDead = value; }
        }

        [SerializeField]
        private GameObject deathEffect;
        public GameObject DeathEffect
        {
            get { return deathEffect; }
            private set { deathEffect = value; }
        }

        [SerializeField]
        private bool isInvulnerable;
        public bool IsInvulnerable
        {
            get { return isInvulnerable; }
            private set { isInvulnerable = value; }
        }


        private void Awake()
        {
            Health = MaxHealth;
        }

        /// <param name="value">Could be both positive and negative value</param>
        public void AddHealth(short value)
        {
            if (value < 0)
            {
                if (!IsInvulnerable)
                {
                    Health = Mathf.Clamp(Health + value, 0, MaxHealth);
                }

                if (Health == 0 && !IsDead)
                {
                    IsDead = true;
                    ///TODO: drop items, destroy gameObject
                    Destroy(gameObject);
                }
                else if (Health != 0)
                {
                    ///TODO: Play sound of taking damage
                    ///TODO: Make Invulnerability
                    //StartCoroutine(MakeInvulnerable(invulnerabilityDuration, invulnerabilityFlickerCount)); 
                }
            }
            else if (value > 0) //healing enemy
            {
                Health = Mathf.Clamp(Health + value, 0, MaxHealth);
                ///TODO: Play sound of healing
                ///TODO: Play healing effect
            }
        }
    }
}
