using Platformer.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private Animator playerAnimator;

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
        }
    }
}
