using Prime31;
using System.Collections;
using UnityEngine;
using static Prime31.CharacterController2D;

namespace Platformer.AI
{
    public class AIGroundMovementTest : Character
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
        private float jumpDelay = 2f;

        [SerializeField]
        private float arriveDelay = 2f;

        [SerializeField]
        private bool canAutoFlipDirection = true;

        [SerializeField]
        private bool canJumpForward = true;

        [SerializeField]
        private bool hasStartedFacingRight = false;

        [SerializeField]
        private Transform[] waypoints;

        private GameObject player;
        private Transform currentWaypoint;
        private Vector3 moveDirection = Vector3.zero;
        private GroundMovementState defaultGroundMovementState;
        private int waypointIndex = 0;
        private bool isGrounded;


        private bool hasChased = false;
        [SerializeField]
        private Transform raycast;
        [SerializeField]
        private LayerMask layerMask;
        [SerializeField]
        private float raycastLength;
        [SerializeField]
        private float attackDistance;
        [SerializeField]
        private float timer;

        private RaycastHit2D hit;
        private float distance;
        private bool hasCooldown = false;
        private float defaultTimer;


        private void Awake()
        {
            defaultTimer = timer;
        }



        // Start is called before the first frame update
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            CharacterController = gameObject.GetComponent<CharacterController2D>();
            defaultGroundMovementState = GroundMovementState;

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
                        if (!currentWaypoint)
                            currentWaypoint = waypoints[waypointIndex];

                        var difference = currentWaypoint.position - transform.position;
                        float distanceX = Mathf.Abs(difference.x);

                        if (distanceX > .1f && difference.x != 0)
                        {
                            moveDirection.x = Mathf.Sign(difference.x) * moveSpeed;

                            transform.eulerAngles = difference.x > 0f ? Vector3.zero : 180 * Vector3.up;

                        }
                        else
                        {
                            StartCoroutine(ArriveAtWaypoint(arriveDelay));
                        }
                        break;
                    case GroundMovementState.Dash:
                        break;
                    case GroundMovementState.Chase:
                        //var tempFacingMultiplier = player.transform.position.x > transform.position.x ? (sbyte)1 : (sbyte)-1;
                        //IsFacingRight = facingMultiplier == tempFacingMultiplier ? IsFacingRight : !IsFacingRight;
                        //facingMultiplier = IsFacingRight ? (sbyte)1 : (sbyte)-1;
                        //OrientateCharacter();
                        //moveDirection.x = facingMultiplier * moveSpeed;
                        //if (!hasChased)
                        //{
                        //    facingMultiplier = player.transform.position.x > transform.position.x ? (sbyte)1 : (sbyte)-1;
                        //    moveDirection.x = facingMultiplier * moveSpeed;
                        //    hasChased = true;
                        //}

                        //transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), moveSpeed * Time.deltaTime / 2);
                        var playerDifference = player.transform.position - transform.position;
                        if (!hasChased)
                        {
                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), moveSpeed * Time.deltaTime);

                            transform.eulerAngles = playerDifference.x > 0f ? Vector3.zero : 180 * Vector3.up;

                            hasChased = true;
                        }
                        break;
                    case GroundMovementState.Attack:
                        //timer = defaultTimer;
                        print("\t\t\tATTACKING");
                        moveDirection = Vector3.zero;
                        break;
                    default:
                        break;
                }
            }

            moveDirection.y -= gravity * Time.deltaTime;

            CharacterController.move(moveDirection * Time.deltaTime);
            Flags = CharacterController.collisionState;

            isGrounded = Flags.below;


            if (GroundMovementState.Equals(GroundMovementState.Chase))
            {
                OrientateCharacter();
            }
            else
            {
                TryOrientateCharacter();
            }

            hit = Physics2D.Raycast(raycast.position, (IsFacingRight ? (sbyte)-1 : (sbyte)1) * Vector2.left, raycastLength, layerMask);
            DebugRaycast();
            if (hit.collider && hit.collider.tag == "Player")
            {
                distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance > attackDistance || hasCooldown)
                {
                    GroundMovementState = GroundMovementState.Chase;
                }
                else
                {
                    GroundMovementState = GroundMovementState.Attack;
                }
            }
            else
            {
                //GroundMovementState = defaultGroundMovementState;
            }
        }

        private void LateUpdate()
        {
            raycast.position = Vector3.MoveTowards(raycast.transform.position, transform.position, 1);
        }

        private void DebugRaycast()
        {
            if (distance > attackDistance)
            {
                Debug.DrawRay(raycast.position, (IsFacingRight ? (sbyte)-1 : (sbyte)1) * Vector2.left * raycastLength, Color.red);
            }
            else if (attackDistance > distance)
            {
                Debug.DrawRay(raycast.position, (IsFacingRight ? (sbyte)-1 : (sbyte)1) * Vector2.left * raycastLength, Color.green);
            }
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
            GroundMovementState = GroundMovementState.Stop;
            yield return new WaitForSeconds(jumpDelay);
            GroundMovementState = GroundMovementState.Jump;
        }

        private IEnumerator ArriveAtWaypoint(float arriveDelay)
        {
            GroundMovementState = GroundMovementState.Stop;
            yield return new WaitForSeconds(arriveDelay);
            if (++waypointIndex > waypoints.Length - 1)
                waypointIndex = 0;

            currentWaypoint = waypoints[waypointIndex];
            GroundMovementState = GroundMovementState.Patrol;
        }

        private IEnumerator ChaseWaiter(float delay)
        {
            yield return new WaitForSeconds(delay);
            GroundMovementState = defaultGroundMovementState;
            hasChased = false;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                GroundMovementState = GroundMovementState.Chase;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                StartCoroutine(ChaseWaiter(2));
            }
        }
    }
}
