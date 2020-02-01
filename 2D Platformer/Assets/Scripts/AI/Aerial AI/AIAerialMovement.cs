using Platformer.Managers;
using System.Collections;
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
        private bool isAlwaysUp = true;

        [SerializeField]
        private float thrust = 10f;

        [SerializeField]
        private float rotationSpeed = 10f;

        [SerializeField]
        private bool isAutoTargetingPlayer = true;

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private float dashSpeed = 5f;

        [SerializeField]
        private float dashDelay = 1f;

        [SerializeField]
        private float floatUpwardsTime = 2f;

        [SerializeField]
        private float floatCooldownTime = 4f;

        [SerializeField]
        private Transform[] waypoints;

        [SerializeField]
        private float distanceThreshold = .1f;

        [SerializeField]
        private bool isRotatingToAttachPoint;

        [SerializeField]
        private bool isRotatinToPlayer;

        [SerializeField]
        private Transform attachPoint;


        private PlayerManager playerStatus;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rigidbody;
        private Transform target;
        private Transform currentWaypoint;
        private float floatTimer;
        private float defaultThrust;
        private float defaultDistanceThreshold;
        private int waypointIndex = 0;
        private bool isFloatingUpwards;
        private bool isTracking = true;
        private bool isMoving = true;


        // Start is called before the first frame update
        private void Start()
        {
            playerStatus = GameObject.FindGameObjectWithTag("Managers").GetComponent<PlayerManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidbody = gameObject.GetComponent<Rigidbody2D>();

            if (isAutoTargetingPlayer)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }

            floatTimer = floatUpwardsTime;
            defaultThrust = thrust;
            defaultDistanceThreshold = distanceThreshold;
        }

        private void FixedUpdate()
        {
            if (playerStatus.IsDead)
            {
                return;
            }

            if (target && isAlwaysUp)
            {
                var direction = transform.position - target.position;
                spriteRenderer.flipY = direction.x > 0;
            }

            switch (aerialMovementState)
            {
                case AerialMovementState.Stop:
                    if (isMoving)
                    {
                        rigidbody.velocity = Vector2.zero;
                        rigidbody.angularVelocity = 0f;
                        isMoving = false;
                    }
                    break;
                case AerialMovementState.Dash:
                    if (isTracking)
                    {
                        LookAt(target);
                    }

                    var hit = Physics2D.Raycast(transform.position, transform.right, 100f, layerMask);
                    if (hit && hit.collider.tag == "Player" && isTracking)
                    {
                        StartCoroutine(Dash(dashDelay, 2 * dashDelay));
                    }
                    break;
                case AerialMovementState.Float:
                    thrust = (isUsingPhysics ? 2.48f : 0.44f) * defaultThrust;
                    rigidbody.gravityScale = 1f;

                    if (isFloatingUpwards)
                        Move(Vector3.up);

                    floatTimer -= Time.deltaTime;
                    if (floatTimer < 0f)
                    {
                        floatTimer = isFloatingUpwards ? floatCooldownTime : floatUpwardsTime;
                        isFloatingUpwards = !isFloatingUpwards;
                    }
                    break;
                case AerialMovementState.MoveTowards:
                    MoveTowards(target);
                    break;
                case AerialMovementState.Move:
                    Move(transform.right);
                    break;
                case AerialMovementState.Patrol:
                    if (!currentWaypoint)
                        currentWaypoint = waypoints[waypointIndex];
                    float distance = Vector3.Distance(currentWaypoint.position, transform.position);
                    if (distance > distanceThreshold)
                    {
                        MoveTowards(currentWaypoint);
                    }
                    else
                    {
                        StartCoroutine(ArriveAtWaypoint());
                    }
                    break;
                case AerialMovementState.Animated:
                    if (attachPoint)
                    {
                        transform.parent = null;
                        float distanceToAttachPoint = Vector3.Distance(attachPoint.position, transform.position);
                        if (distanceToAttachPoint > distanceThreshold)
                        {
                            MoveTowards(attachPoint);
                        }
                        else
                        {
                            rigidbody.velocity = Vector2.zero;
                            rigidbody.angularVelocity = 0f;
                            transform.position = attachPoint.position;
                            if (isRotatingToAttachPoint)
                            {
                                transform.rotation = attachPoint.rotation;
                            }
                            else if (isRotatinToPlayer)
                            {
                                LookAt(target);
                            }
                            else
                            {
                                transform.rotation = Quaternion.identity;
                            }
                        }
                        distanceThreshold = distanceToAttachPoint < 10 ? 50 * defaultDistanceThreshold : defaultDistanceThreshold;
                    }
                    else
                    {
                        Debug.LogWarning("No attachment point selected.");
                    }
                    break;
                default:
                    break;
            }

            if (!aerialMovementState.Equals(AerialMovementState.Float))
            {
                thrust = defaultThrust;
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
                rigidbody.velocity = Vector2.zero;
                rigidbody.angularDrag = 0f;
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
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, rotationSpeed * Time.deltaTime);
        }

        private IEnumerator Dash(float delayBeforeDash = 1f, float delayAfterDash = 2f)
        {
            isTracking = false;
            yield return new WaitForSeconds(delayBeforeDash);
            rigidbody.AddForce(transform.right * dashSpeed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(delayAfterDash);
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0f;
            isTracking = true;
        }

        private IEnumerator ArriveAtWaypoint(float delay = 2f)
        {
            aerialMovementState = AerialMovementState.Stop;
            yield return new WaitForSeconds(delay);
            if (++waypointIndex > waypoints.Length - 1)
                waypointIndex = 0;
            currentWaypoint = waypoints[waypointIndex];
            aerialMovementState = AerialMovementState.Patrol;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collisionBehaviour)
            {
                case CollisionBehaviour.None:
                    return;
                case CollisionBehaviour.Rebound:
                    var reflectedPosition = Vector2.Reflect(transform.right, collision.contacts[0].normal);
                    rigidbody.velocity = reflectedPosition.normalized * thrust;
                    Vector3 direction = rigidbody.velocity;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    rigidbody.freezeRotation = false;
                    rigidbody.MoveRotation(angle);
                    rigidbody.angularVelocity = 0f;
                    break;
                case CollisionBehaviour.Fall:
                    rigidbody.gravityScale = 9.81f;
                    aerialMovementState = AerialMovementState.Stop;
                    break;
                case CollisionBehaviour.Explode:
                    Destroy(gameObject); ///TODO: Add Explode effect/particles + change this with object pooling
                    break;
                case CollisionBehaviour.Disappear:
                    Destroy(gameObject); ///TODO: Change this with object pooling
                    break;
            }
        }
    }
}
