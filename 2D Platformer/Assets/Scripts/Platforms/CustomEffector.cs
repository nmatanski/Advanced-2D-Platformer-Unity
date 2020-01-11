using UnityEngine;

namespace Platformer
{
    public class CustomEffector : MonoBehaviour
    {
        [SerializeField]
        private EffectorType effectorType;
        public EffectorType EffectorType
        {
            get { return effectorType; }
            private set { effectorType = value; }
        }

        [SerializeField]
        private Vector3 effectorAdjustment;
        public Vector3 EffectorAdjustment
        {
            get { return effectorAdjustment; }
            private set { effectorAdjustment = value; }
        }

        [SerializeField]
        private bool isDisabledGroundSlamming;
        public bool IsDisabledGroundSlamming
        {
            get { return isDisabledGroundSlamming; }
            private set { isDisabledGroundSlamming = value; }
        }
    }
}
