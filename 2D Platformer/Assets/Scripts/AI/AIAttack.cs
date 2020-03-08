using Platformer.Managers;
using UnityEngine;

namespace Platformer.AI
{
    public class AIAttack : MonoBehaviour
    {
        [SerializeField]
        private short attackDamage = 10;

        [SerializeField]
        private bool isCollider = true;

        [Tooltip("True: Will not use the default OnTrigger/OnCollision code; Collisions with player should be managed from another script!")]
        [SerializeField]
        private bool isManagedByAIScript = false;


        private PlayerManager playerStatus;


        private void Start()
        {
            playerStatus = Managers.Managers.Player;
        }


        public void DealDamageToPlayer(Collision2D collision)
        {
            playerStatus.AddHealth((short)-attackDamage);

            playerStatus.GetComponent<ModifyTime>().StopTime();
            playerStatus.LastAttackDirection = collision.gameObject.transform.position - transform.position;
        }

        public void DealDamageToPlayer(Collider2D triggerCollision)
        {
            playerStatus.AddHealth((short)-attackDamage);

            playerStatus.GetComponent<ModifyTime>().StopTime();
            playerStatus.LastAttackDirection = triggerCollision.gameObject.transform.position - transform.position;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && isCollider && !isManagedByAIScript)
            {
                DealDamageToPlayer(collision);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !isCollider && !isManagedByAIScript)
            {
                DealDamageToPlayer(collision);
            }
        }
    }
}
