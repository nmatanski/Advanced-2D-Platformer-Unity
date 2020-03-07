using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private Animator playerAnimator;

        [SerializeField]
        private Collider2D weaponCollider;

        [SerializeField]
        private int weaponDamage;

        [SerializeField]
        private LayerMask enemyLayers;


        private void Start()
        {
            playerAnimator = Managers.Managers.Player.PlayerAnimator;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
        }

        private void Attack()
        {
            playerAnimator.SetTrigger("Attack");

            var contactFilter = new ContactFilter2D();
            contactFilter.layerMask = enemyLayers;
            contactFilter.useLayerMask = true;
            var attackedEnemyColliders = new List<Collider2D>();

            Physics2D.OverlapCollider(weaponCollider, contactFilter, attackedEnemyColliders);

            foreach (var attackedEnemyCollider in attackedEnemyColliders)
            {
#pragma warning disable S3923 // All branches in a conditional structure should not have exactly the same implementation
                switch (attackedEnemyCollider.tag)
                {
                    case "Weapon":
                        ///TODO: raise flag for weapon being hit, then check if we have its owner in the List
                        ///TODO: add a Weapon script to the weapon game object with the enemy/player gameobject and script to know who has this weapon
                        break;
                    case "Enemy":
                        //attackedEnemyCollider.GetComponent<Enemy>().TakeDamage(weaponDamage); ///TODO: Use enemy script here
                        break;
                    default:
                        break;
                }
#pragma warning restore S3923 // All branches in a conditional structure should not have exactly the same implementation
                Debug.Log($"We hit a {attackedEnemyCollider.name}");
            }

            ///TODO: check if we have a weapon and its owner being collided to call Clash/Parry/Block/Stun method/s
        }
    }
}
