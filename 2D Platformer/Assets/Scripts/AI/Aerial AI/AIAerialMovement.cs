using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.AI
{
    public class AIAerialMovement : MonoBehaviour
    {
        [SerializeField]
        private AerialMovementState aerialMovementState;

        [SerializeField]
        private CollisionBehaviour collisionBehaviour;

        [SerializeField]
        private bool isUsingPhysics = true;

        [SerializeField]
        private float thrust = 10f;

        [SerializeField]
        private float rotationSpeed = 10f;

        [SerializeField]
        private bool isAutoTargetingPlayer = true;


        private Rigidbody2D rigidbody;
        private Transform target;


        // Start is called before the first frame update
        private void Start()
        {
            rigidbody = gameObject.GetComponent<Rigidbody2D>();

            if (isAutoTargetingPlayer)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void FixedUpdate()
        {
            switch (aerialMovementState)
            {
                case AerialMovementState.Stop:
                    rigidbody.velocity = Vector2.zero;
                    rigidbody.angularVelocity = 0f;
                    break;
                case AerialMovementState.Dash:
                    break;
                case AerialMovementState.Float:
                    break;
                case AerialMovementState.MoveTowards:
                    MoveTowards(target);
                    break;
                case AerialMovementState.Move:
                    Move(transform.right);
                    break;
                case AerialMovementState.Patrol:
                    break;
                case AerialMovementState.Animated:
                    break;
                default:
                    break;
            }
        }

        public void Move(Vector3 moveDirection)
        {
            if (isUsingPhysics)
            {
                rigidbody.AddForce(moveDirection * thrust);
            }
            else
            {
                rigidbody.MovePosition(transform.position + moveDirection * thrust * Time.deltaTime);
            }
        }

        public void MoveTowards(Transform target)
        {
            LookAt(target);
            Move(transform.right);
        }

        private void LookAt(Transform target)
        {
            var direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.y) * Mathf.Rad2Deg;
            var quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, rotationSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

        }
    }
}
