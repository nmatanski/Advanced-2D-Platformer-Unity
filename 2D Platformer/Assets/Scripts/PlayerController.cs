using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using static Prime31.CharacterController2D;
using System;

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
        private float wallJumpSpeedMultiplierX = 2f;

        [SerializeField]
        private float wallJumpSpeedMultiplierY = 2f;

        [SerializeField]
        private float wallJumpRecoveryTime = .2f;

        [SerializeField]
        private float wallRunDuration = .5f;

        [SerializeField]
        private float wallRunSpeed = 2f;

        [SerializeField]
        private float slideSpeed = 4f;

        [SerializeField]
        private float gravity = 20f;

        [SerializeField]
        private float jumpPressedRememberTime = .1f;

        [SerializeField]
        private float groundedRememberTime = .1f;

        [SerializeField]
        [Range(0f, .99f)]
        private float horizontalDamping = .2f; //0 = no damping

        [SerializeField]
        private LayerMask layerMask;


        //cached components
        private CharacterController2D characterController;


        //ability toggles

        public bool CanDoubleJump { get; private set; } = true;

        public bool CanWallJump { get; private set; } = true;

        public bool CanWallRun { get; private set; } = true;

        public bool CanWallRunAfterWallJump { get; private set; } = true;

        public bool CanSlide { get; private set; } = true;


        //state

        public bool IsGrounded { get; private set; }

        public bool IsJumping { get; private set; }

        public bool IsWallRunning { get; private set; }

        public bool IsFacingRight { get; private set; }

        public bool HasDoubleJumped { get; private set; }

        public bool HasWallJumped { get; private set; }

        public bool IsSliding { get; private set; }


        public CharacterCollisionState2D Flags { get; private set; }

        private float jumpPressedRemember = 0f;
        private float groundedRemember = 0f;


        private Vector3 moveDirection = Vector3.zero;
        private bool wasLastJumpLeft;
        private float slopeAngle;
        private Vector3 slopeGradient = Vector3.zero;


        private void Start()
        {
            characterController = GetComponent<CharacterController2D>();
        }

        private void Update()
        {
            Run();
            CheckForSliding();
            OrientatePlayer();

            groundedRemember -= Time.deltaTime;

            if (IsGrounded)
            {
                ResetTimer(ref groundedRemember, groundedRememberTime);
                ResetJump();

                TrySlopeSliding();

                if (Input.GetButtonDown("Jump"))
                {
                    //ActivateJump(jumpSpeed);
                    IsJumping = true;
                    IsWallRunning = true;
                }
            }
            else
            {
                ActivateSmartJumpWithHeightCut();
                ActivateDoubleJump();
            }

            TryJumpWithHelper(jumpSpeed);

            ApplyGravity();

            characterController.move(moveDirection * Time.deltaTime);

            Flags = characterController.collisionState;
            IsGrounded = Flags.below;

            if (Flags.above) //ceiling
            {
                if (moveDirection.y > 0)
                {
                    moveDirection.y = 0;
                }

                ApplyGravity();
            }

            if (Flags.left || Flags.right) //left/right walls
            {
                TryWallRun();
                TryWallJump();
            }
            else
            {
                TryWallRunAfterWallJump();
            }

            jumpPressedRemember -= Time.deltaTime;

            if (Input.GetButtonDown("Jump"))
            {
                ResetTimer(ref jumpPressedRemember, jumpPressedRememberTime);
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
            if (HasWallJumped)
            {
                return;
            }

            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.x *= walkSpeed * Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);
        }

        private void TryWallRun()
        {
            if (CanWallRun && Input.GetAxis("Vertical") > 0 && IsWallRunning)
            {
                moveDirection.y = jumpSpeed / wallRunSpeed;
                StartCoroutine(WallRunDurationTimer(wallRunDuration));
            }
        }

        private void TryWallRunAfterWallJump()
        {
            if (CanWallRunAfterWallJump)
            {
                StopCoroutine(WallRunDurationTimer(wallRunDuration));
                IsWallRunning = true;
            }
        }

        private void CheckForSliding()
        {
            var hit = Physics2D.Raycast(transform.position, Vector3.down, 2f, layerMask);

            if (hit)
            {
                slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                slopeGradient = hit.normal;

                if (slopeAngle > characterController.slopeLimit)
                {
                    IsSliding = true;
                }
                else
                {
                    IsSliding = false;
                }
            }
        }

        private void TrySlopeSliding()
        {
            if (IsSliding)
            {
                moveDirection = slideSpeed * new Vector3(slopeGradient.x, -slopeGradient.y, 0f);
            }
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

        private bool TryJumpWithHelper(float speed)
        {
            if (jumpPressedRemember > 0 && groundedRemember > 0)
            {
                jumpPressedRemember = 0;
                groundedRemember = 0;
                ActivateJump(speed);
                return true;
            }
            return false;
        }

        private bool TryWallJump()
        {
            if (CanWallJump && Input.GetButtonDown("Jump") && !HasWallJumped && !IsGrounded)
            {
                float totalSpeedX = jumpSpeed * wallJumpSpeedMultiplierX;
                float totalSpeedY = jumpSpeed * wallJumpSpeedMultiplierY;
                //ActivateJump(totalSpeedY);
                if (!TryJumpWithHelper(totalSpeedY)) ///TODO: bug: not applying the assist method
                {
                    ActivateJump(totalSpeedY);
                }
                if (moveDirection.x < 0)
                {
                    moveDirection.x = totalSpeedX;
                    transform.eulerAngles = Vector3.zero;
                    wasLastJumpLeft = false;
                }
                else if (moveDirection.x > 0)
                {
                    moveDirection.x = -totalSpeedX;
                    transform.eulerAngles = 180 * Vector3.up;
                    wasLastJumpLeft = true;
                }
                StartCoroutine(WallJumpRecoveryTimer(wallJumpRecoveryTime));
                HasDoubleJumped = false;
                CanDoubleJump = true;
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

        private IEnumerator WallJumpRecoveryTimer(float waitTime)
        {
            HasWallJumped = true;
            yield return new WaitForSeconds(waitTime);
            HasWallJumped = false;
        }

        private IEnumerator WallRunDurationTimer(float duration)
        {
            IsWallRunning = true;
            yield return new WaitForSeconds(duration);
            IsWallRunning = false;
        }
    }
}
