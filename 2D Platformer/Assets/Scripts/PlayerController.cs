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
        [Header("CONFIGURATION")]

        [SerializeField]
        private float walkSpeed = 6f;

        [SerializeField]
        private float crouchWalkSpeed = 3f;

        [SerializeField]
        private float jumpSpeed = 8f;

        [SerializeField]
        private float doubleJumpSpeed = 4f;

        [SerializeField]
        private float powerJumpSpeed = 10f;

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
        private float glideGravity = 2f;

        [SerializeField]
        private float glideDurationCapacity = 2f;

        [SerializeField]
        private int glideCharges = 2;

        [SerializeField]
        private float groundSlamSpeed = 4f;

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


        //ability toggles
        [Header("ABILITIES")]

        [SerializeField]
        private bool canDoubleJump = true;
        public bool CanDoubleJump { get => canDoubleJump; private set => canDoubleJump = value; }

        [SerializeField]
        private bool canWallJump = true;
        public bool CanWallJump { get => canWallJump; private set => canWallJump = value; }

        [SerializeField]
        private bool canWallRun = true;
        public bool CanWallRun { get => canWallRun; private set => canWallRun = value; }

        [SerializeField]
        private bool canWallRunAfterWallJump = true;
        public bool CanWallRunAfterWallJump { get => canWallRunAfterWallJump; private set => canWallRunAfterWallJump = value; }

        [SerializeField]
        private bool canSlide = true;
        public bool CanSlide { get => canSlide; private set => canSlide = value; }

        [SerializeField]
        private bool canGlide = true;
        public bool CanGlide { get => canGlide; private set => canGlide = value; }

        [SerializeField]
        private bool canPowerJump = true;
        public bool CanPowerJump
        {
            get { return canPowerJump; }
            private set { canPowerJump = value; }
        }

        [SerializeField]
        private bool canGroundSlam = true;
        public bool CanGroundSlam
        {
            get { return canGroundSlam; }
            private set { canGroundSlam = value; }
        }




        //state
        [Header("STATE")]

        [SerializeField]
        private bool isGrounded;
        public bool IsGrounded { get => isGrounded; private set => isGrounded = value; }

        [SerializeField]
        private bool isJumping;
        public bool IsJumping { get => isJumping; private set => isJumping = value; }

        [SerializeField]
        private bool isWallRunning;
        public bool IsWallRunning { get => isWallRunning; private set => isWallRunning = value; }

        [SerializeField]
        private bool isFacingRight;
        public bool IsFacingRight { get => isFacingRight; private set => isFacingRight = value; }

        [SerializeField]
        private bool hasDoubleJumped;
        public bool HasDoubleJumped { get => hasDoubleJumped; private set => hasDoubleJumped = value; }

        [SerializeField]
        private bool hasWallJumped;
        public bool HasWallJumped { get => hasWallJumped; private set => hasWallJumped = value; }

        [SerializeField]
        private bool isSliding;
        public bool IsSliding { get => isSliding; private set => isSliding = value; }

        [SerializeField]
        private bool isGliding;
        public bool IsGliding { get => isGliding; private set => isGliding = value; }

        [SerializeField]
        private bool isDucking;
        public bool IsDucking
        {
            get { return isDucking; }
            private set { isDucking = value; }
        }

        [SerializeField]
        private bool isCrouchWalking;
        public bool IsCrouchWalking
        {
            get { return isCrouchWalking; }
            private set { isCrouchWalking = value; }
        }

        [SerializeField]
        private bool isPowerJumping;
        public bool IsPowerJumping
        {
            get { return isPowerJumping; }
            private set { isPowerJumping = value; }
        }

        [SerializeField]
        private bool isGroundSlamming;
        public bool IsGroundSlamming
        {
            get { return isGroundSlamming; }
            set { isGroundSlamming = value; }
        }



        public CharacterCollisionState2D Flags { get; private set; }

        //cached components
        private CharacterController2D characterController;
        private BoxCollider2D boxCollider;

        //private variables
        private float jumpPressedRemember = 0f;
        private float groundedRemember = 0f;
        private Vector3 moveDirection = Vector3.zero;
        private bool wasLastJumpLeft;
        private float slopeAngle;
        private Vector3 slopeGradient = Vector3.zero;
        private bool hasStartedGliding;
        private float remainingGlideTime;
        private bool isGliderEquipped = true;
        private Vector2 defaultBoxColliderSize;
        private Vector3 frontTopCorner;
        private Vector3 backTopCorner;
        private float defaultJumpSpeed;


        private void Start()
        {
            characterController = GetComponent<CharacterController2D>();
            boxCollider = GetComponent<BoxCollider2D>();

            defaultBoxColliderSize = boxCollider.size;
            defaultJumpSpeed = jumpSpeed;

            ResetTimer(ref remainingGlideTime, glideDurationCapacity);
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
                EquipGlider();
                ResetJump();
                IsGroundSlamming = false;
                TrySlopeSliding();

                if (Input.GetButtonDown("Jump"))
                {
                    if (CanPowerJump && (IsDucking || IsCrouchWalking))
                    {
                        jumpSpeed += powerJumpSpeed;
                        StartCoroutine(PowerJumpWaiter(1f));
                    }
                    else
                    {
                        IsJumping = true;
                    }

                    IsWallRunning = true;
                }
            }
            else
            {
                jumpSpeed = defaultJumpSpeed;
                ActivateSmartJumpWithHeightCut();
                ActivateDoubleJump();
            }

            TryJumpWithHelper(jumpSpeed);
            ProcessGravity();

            characterController.move(moveDirection * Time.deltaTime);

            Flags = characterController.collisionState;
            IsGrounded = Flags.below;


            frontTopCorner = new Vector3(transform.position.x + boxCollider.size.x / 2, transform.position.y + boxCollider.size.y / 2, 0);
            backTopCorner = new Vector3(transform.position.x - boxCollider.size.x / 2, transform.position.y + boxCollider.size.y / 2, 0);

            var hitFrontCeiling = Physics2D.Raycast(frontTopCorner, Vector2.up, 2f, layerMask);
            var hitBackCeiling = Physics2D.Raycast(backTopCorner, Vector2.up, 2f, layerMask);

            TryDuckOrCrouchWalk(hitFrontCeiling, hitBackCeiling);

            if (Flags.above) //ceiling
            {
                if (moveDirection.y > 0)
                {
                    moveDirection.y = 0;
                }

                ApplyGravity(gravity);
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

        private void TryDuckOrCrouchWalk(RaycastHit2D hitFrontCeiling, RaycastHit2D hitBackCeiling)
        {
            if (Input.GetAxis("Vertical") < 0 && moveDirection.x == 0) ///TODO: Change duck key bind from vertical negative to Ctrl for example
            {
                TryDuckOrCrouchWalk();
                IsDucking = true;
                IsCrouchWalking = false;
            }
            else if (Input.GetAxis("Vertical") < 0 && moveDirection.x != 0) ///TODO: Change duck key
            {
                TryDuckOrCrouchWalk();
                IsDucking = false;
                IsCrouchWalking = true;
            }
            else
            {
                if (!hitFrontCeiling.collider && !hitBackCeiling.collider && (isDucking || IsCrouchWalking))
                {
                    ChangeBoxColliderSize(boxCollider, new Vector2(boxCollider.size.x, defaultBoxColliderSize.y), true);
                    IsDucking = false;
                    IsCrouchWalking = false;
                }
            }
        }

        private void TryDuckOrCrouchWalk()
        {
            if (!IsDucking && !IsCrouchWalking)
                ChangeBoxColliderSize(boxCollider, new Vector2(boxCollider.size.x, defaultBoxColliderSize.y / 2));
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

        private void EquipGlider()
        {
            if (glideCharges > 0 && Input.GetKeyDown(KeyCode.T) && !isGliderEquipped)
            {
                ResetTimer(ref remainingGlideTime, glideDurationCapacity);
                isGliderEquipped = true;
            }
        }

        private void ProcessGravity()
        {
            float gravityType;

            if (CanGlide && Input.GetAxis("Vertical") > .5f && characterController.velocity.y < .2f) ///TODO: Change key
            {
                if (remainingGlideTime > 0)
                {
                    IsGliding = true;

                    if (hasStartedGliding)
                    {
                        moveDirection.y = 0;
                        hasStartedGliding = false;
                    }

                    gravityType = glideGravity;

                    remainingGlideTime -= Time.deltaTime; ///TODO: Visualize glider remaining time in UI
                }
                else
                {
                    isGliding = false;

                    if (isGliderEquipped)
                    {
                        Mathf.Clamp(--glideCharges, 0, int.MaxValue);
                    }

                    isGliderEquipped = false;

                    gravityType = gravity;
                }
            }
            else if (CanGroundSlam && IsDucking && !IsPowerJumping)
            {
                gravityType = 0;
                ApplyGravity(gravity, groundSlamSpeed);
                IsGroundSlamming = true;
            }
            else
            {
                isGliding = false;
                hasStartedGliding = true;

                gravityType = gravity;
            }

            if (gravityType != 0)
            {
                ApplyGravity(gravityType);
            }
        }

        private void ApplyGravity(float gravity, float bonusGravity = 0)
        {
            moveDirection.y -= gravity * Time.deltaTime + bonusGravity;
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

        private void ChangeBoxColliderSize(BoxCollider2D collider, Vector2 newSize, bool isCrouching = false)
        {
            var multiplier = isCrouching ? 1 : -1;
            collider.size = newSize;
            transform.position = new Vector3(transform.position.x, transform.position.y + multiplier * (defaultBoxColliderSize.y / 4), transform.position.z);
            characterController.recalculateDistanceBetweenRays();
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

        private IEnumerator PowerJumpWaiter(float delay)
        {
            IsPowerJumping = true;
            yield return new WaitForSeconds(delay);
            IsPowerJumping = false;
        }
    }
}
