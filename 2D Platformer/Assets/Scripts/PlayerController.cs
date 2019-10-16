using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using static Prime31.CharacterController2D;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        //config

        [SerializeField]
        private float walkSpeed = 6f;

        [SerializeField]
        private float jumpSpeed = 8f;

        [SerializeField]
        private float doubleJumpSpeed = 4f;

        [SerializeField]
        private float gravity = 20f;

        [SerializeField]
        private float jumpPressedRememberTime = .1f;

        [SerializeField]
        private float groundedRememberTime = .1f;

        [SerializeField]
        [Range(0f, .99f)]
        private float horizontalDamping = .2f; //0 = no damping


        //state

        public bool CanDoubleJump { get; private set; } = true;


        public bool IsGrounded { get; private set; }

        public bool IsJumping { get; private set; }

        public bool IsFacingRight { get; private set; }

        public bool HasDoubleJumped { get; private set; }


        public CharacterCollisionState2D Flags { get; private set; }

        private CharacterController2D characterController;
        private Vector3 moveDirection = Vector3.zero;
        private float jumpPressedRemember = 0f;
        private float groundedRemember = 0f;


        private void Start()
        {
            characterController = GetComponent<CharacterController2D>();
        }

        private void Update()
        {
            Run();

            OrientatePlayer();

            groundedRemember -= Time.deltaTime;

            if (IsGrounded)
            {
                ResetTimer(ref groundedRemember, groundedRememberTime);

                ResetJump();

                if (Input.GetButtonDown("Jump"))
                {
                    ActivateJump(jumpSpeed);
                    IsJumping = true;
                }
            }
            else
            {
                ActivateSmartJumpWithHeightCut();
                ActivateDoubleJump();
            }

            jumpPressedRemember -= Time.deltaTime;

            if (Input.GetButtonDown("Jump"))
            {
                ResetTimer(ref jumpPressedRemember, jumpPressedRememberTime);
            }

            TryJumpWithHelper();

            ApplyGravity();

            characterController.move(moveDirection * Time.deltaTime);

            Flags = characterController.collisionState;
            IsGrounded = Flags.below;

            if (Flags.above)
            {
                ApplyGravity();
            }
        }

        private void ActivateDoubleJump()
        {
            if (Input.GetButtonDown("Jump") && CanDoubleJump && !HasDoubleJumped)
            {
                ActivateJump(doubleJumpSpeed);
                HasDoubleJumped = true;
            }
        }

        private void Run()
        {
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.x *= walkSpeed * Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);
        }

        private void ActivateJump(float speed)
        {
            moveDirection.y = speed;
        }

        private void ActivateSmartJumpWithHeightCut()
        {
            if (Input.GetButtonUp("Jump") && moveDirection.y > 0)
            {
                moveDirection.y *= .5f;
            }
        }

        private void ResetJump()
        {
            moveDirection.y = 0;
            IsJumping = false;
            HasDoubleJumped = false;
        }

        private bool TryJumpWithHelper()
        {
            if (jumpPressedRemember > 0 && groundedRemember > 0)
            {
                jumpPressedRemember = 0;
                groundedRemember = 0;
                ActivateJump(jumpSpeed);
                return true;
            }
            return false;
        }

        private void ApplyGravity()
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        private void OrientatePlayer()
        {
            if (moveDirection.x < 0)
            {
                transform.eulerAngles = 180 * Vector3.up;
                IsFacingRight = false;
            }
            else if (moveDirection.x > 0)
            {
                transform.eulerAngles = Vector3.zero;
                IsFacingRight = true;
            }
        }

        private void ResetTimer(ref float currentTimer, float defaultTimer)
        {
            currentTimer = defaultTimer;
        }
    }
}
