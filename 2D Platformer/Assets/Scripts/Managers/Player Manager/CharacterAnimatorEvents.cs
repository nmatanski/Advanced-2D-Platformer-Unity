using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class CharacterAnimatorEvents : MonoBehaviour
    {
        [SerializeField]
        private PlayerCombat playerCombat;

        public void ProcessAttackData()
        {
            playerCombat.ProcessAttackData();
        }
    }
}
