using UnityEngine;
using Prime31;
using System.Collections;
using static Prime31.CharacterController2D;


namespace Platformer.AI
{
    public class AIGroundMovement : Character
    {
        [SerializeField]
        private GroundMovementState groundMovementState;
        public GroundMovementState GroundMovementState
        {
            get { return groundMovementState; }
            private set { groundMovementState = value; }
        }

        [SerializeField]
        private float jumpSpeed = 10f;
        public override float JumpSpeed
        {
            get { return jumpSpeed; }
            set { jumpSpeed = value; }
        }


        public override bool IsFacingRight { get; protected set; } = true;

        public override CharacterCollisionState2D Flags { get; protected set; }

        protected override CharacterController2D CharacterController { get; set; }


        [SerializeField]
        private float moveSpeed = 5f;

        [SerializeField]
        private float gravity = 20f;

        [SerializeField]
        private float jumpDelay = 0f;

        [SerializeField]
        private bool canAutoFlipDirection = true;

        [SerializeField]
        private bool canJumpForward = true;

        [SerializeField]
        private bool hasStartedFacingRight = false;


        private Vector3 moveDirection = Vector3.zero;
        private bool isGrounded;



        // Start is called before the first frame update
        private void Start()
        {
            CharacterController = gameObject.GetComponent<CharacterController2D>();
            if (!hasStartedFacingRight)
            {
                transform.eulerAngles = 180 * Vector3.up;
                IsFacingRight = false;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (isGrounded)
            {
                var facingMultiplier = IsFacingRight ? (sbyte)1 : (sbyte)-1;

                switch (GroundMovementState)
                {
                    case GroundMovementState.Stop:
                        moveDirection = Vector3.zero;
                        break;
                    case GroundMovementState.MoveForward:
                        //TryOrientateCharacter();
                        moveDirection.x = facingMultiplier * moveSpeed;
                        break;
                    case GroundMovementState.Jump:
                        //TryOrientateCharacter();
                        moveDirection.y = JumpSpeed;
                        if (canJumpForward)
                            moveDirection.x = facingMultiplier * moveSpeed;
                        if (jumpDelay > 0)
                        {
                            StartCoroutine(JumpWaiter(jumpDelay));
                        }
                        break;
                    case GroundMovementState.Patrol:
                        break;
                    case GroundMovementState.Dash:
                        break;
                    default:
                        break;
                }
            }

            moveDirection.y -= gravity * Time.deltaTime;

            CharacterController.move(moveDirection * Time.deltaTime);
            Flags = CharacterController.collisionState;

            isGrounded = Flags.below;

            TryOrientateCharacter();
        }

        protected override void OrientateCharacter()
        {
            if (IsFacingRight)
            {
                transform.eulerAngles = 180 * Vector3.up;
                IsFacingRight = false;
            }
            else if (!IsFacingRight)
            {
                transform.eulerAngles = Vector3.zero;
                IsFacingRight = true;
            }

            moveDirection.x *= -1; //mid-air flip
        }

        private void TryOrientateCharacter()
        {
            if (canAutoFlipDirection && ((Flags.left && !IsFacingRight) || (Flags.right && IsFacingRight)))
            {
                OrientateCharacter();
            }
        }

        private IEnumerator JumpWaiter(float jumpDelay)
        {
            groundMovementState = GroundMovementState.Stop;
            yield return new WaitForSeconds(jumpDelay);
            groundMovementState = GroundMovementState.Jump;
        }
    }
}
