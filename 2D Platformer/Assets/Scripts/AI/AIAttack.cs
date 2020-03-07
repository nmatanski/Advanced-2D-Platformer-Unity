using System;
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


        public void DealDamageToPlayer(Collision2D collision)
        {
            playerStatus.AddHealth((short)-attackDamage);

            playerStatus.GetComponent<ModifyTime>().StopTime();
            playerStatus.LastAttackDirection = collision.gameObject.transform.position - transform.position;
        }

        public void DealDamageToPlayer(Collider2D collision)
        {
            playerStatus.AddHealth((short)-attackDamage);

            playerStatus.GetComponent<ModifyTime>().StopTime();
            playerStatus.LastAttackDirection = collision.gameObject.transform.position - transform.position;
        }


        private void OnCollisionEnter2D(Collision2D collision) ///TODO: Stay when player gets invulnerability   - fixed in player? needs tests
        {
            if (collision.gameObject.CompareTag("Player") && isCollider)
            {
                DealDamageToPlayer(collision);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision) ///TODO: Stay when player gets invulnerability   - fixed in player? needs tests
        {
            if (collision.gameObject.CompareTag("Player") && !isCollider)
            {
                DealDamageToPlayer(collision);
            }
        }
    }
}
