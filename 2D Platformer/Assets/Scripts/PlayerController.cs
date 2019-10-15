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

        [SerializeField]
        private bool isJumping;


        //state

        public CharacterCollisionState2D Flags { get; set; }

        private CharacterController2D characterController;
        private Vector3 moveDirection = Vector3.zero;


        private void Start()
        {
            characterController = GetComponent<CharacterController2D>();
        }

        private void Update()
        {
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.x *= walkSpeed;

            if (isGrounded)
            {
                moveDirection.y = 0;
                isJumping = false;

                if (moveDirection.x < 0)
                {
                    transform.eulerAngles = 180 * Vector3.up;
                }
                else
                {
                    transform.eulerAngles = Vector3.zero;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                    isJumping = true;
                }
            }
            else
            {
                if (Input.GetButtonUp("Jump") && moveDirection.y > 0)
                {
                    moveDirection.y *= .5f;
                }
            }

            moveDirection.y -= gravity * Time.deltaTime;
            characterController.move(moveDirection * Time.deltaTime);

            Flags = characterController.collisionState;
            isGrounded = Flags.below;

            if (Flags.above)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
        }
    }
}
