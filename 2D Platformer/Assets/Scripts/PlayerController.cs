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
        private float gravity = 20f;

        [SerializeField]
        private bool isGrounded;
        public bool IsGrounded
        {
            get { return isGrounded; }
            private set { isGrounded = value; }
        }

        [SerializeField]
        private bool isJumping;
        public bool IsJumping
        {
            get { return isJumping; }
            private set { isJumping = value; }
        }

        [SerializeField]
        private bool isFacingRight;
        public bool IsFacingRight
        {
            get { return isFacingRight; }
            private set { isFacingRight = value; }
        }

        [SerializeField]
        private float jumpPressedRememberTime = .1f;

        [SerializeField]
        private float groundedRememberTime = .1f;

        [SerializeField]
        [Range(0f, .99f)]
        private float horizontalDamping = .2f; //0 = no damping


        //state

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
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.x *= walkSpeed * Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);

            groundedRemember -= Time.deltaTime;

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

            if (IsGrounded)
            {
                groundedRemember = groundedRememberTime;

                moveDirection.y = 0;
                IsJumping = false;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                    IsJumping = true;
                }
            }
            else
            {
                if (Input.GetButtonUp("Jump") && moveDirection.y > 0)
                {
                    moveDirection.y *= .5f;
                }
            }

            jumpPressedRemember -= Time.deltaTime;

            if (Input.GetButtonDown("Jump"))
            {
                jumpPressedRemember = jumpPressedRememberTime;
            }

            if (jumpPressedRemember > 0 && groundedRemember > 0)
            {
                jumpPressedRemember = 0;
                groundedRemember = 0;
                moveDirection.y = jumpSpeed;
                IsJumping = true; ///TODO: is this required?
            }

            moveDirection.y -= gravity * Time.deltaTime;
            characterController.move(moveDirection * Time.deltaTime);

            Flags = characterController.collisionState;
            IsGrounded = Flags.below;

            if (Flags.above)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
        }
    }
}
