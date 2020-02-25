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

        private PlayerManager playerStatus;


        private void Start()
        {
            //playerStatus = GameObject.FindGameObjectWithTag("Managers").GetComponent<PlayerManager>();
            playerStatus = Managers.Managers.Player;
        }


        private void OnCollisionEnter2D(Collision2D collision) ///TODO: Stay when player gets invulnerability
        {
            if (collision.gameObject.CompareTag("Player") && isCollider)
            {
                playerStatus.AddHealth((short)-attackDamage);

                playerStatus.GetComponent<ModifyTime>().StopTime();
                playerStatus.LastAttackDirection = collision.gameObject.transform.position - transform.position;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision) ///TODO: Stay when player gets invulnerability
        {
            if (collision.gameObject.CompareTag("Player") && !isCollider)
            {
                playerStatus.AddHealth((short)-attackDamage);

                playerStatus.GetComponent<ModifyTime>().StopTime();
                playerStatus.LastAttackDirection = collision.gameObject.transform.position - transform.position;
            }
        }
    }
}
