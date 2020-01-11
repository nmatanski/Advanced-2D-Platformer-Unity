using UnityEngine;

namespace Platformer
{
    public class JumpPad : MonoBehaviour
    {
        [SerializeField]
        private float jumpPadSpeed = 15f;
        public float JumpPadSpeed
        {
            get { return jumpPadSpeed; }
            private set { jumpPadSpeed = value; }
        }

    }
}
