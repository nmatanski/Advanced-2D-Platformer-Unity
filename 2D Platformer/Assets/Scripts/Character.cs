using Prime31;
using UnityEngine;
using static Prime31.CharacterController2D;

namespace Platformer
{
    public abstract class Character : MonoBehaviour
    {
        public abstract float JumpSpeed { get; set; }

        public abstract CharacterCollisionState2D Flags { get; protected set; }

        public abstract bool IsFacingRight { get; protected set; }

        protected abstract CharacterController2D CharacterController { get; set; }


        protected abstract void OrientateCharacter();
    }
}
