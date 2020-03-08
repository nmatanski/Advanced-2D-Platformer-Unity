using System.Collections;
using Prime31;
using UnityEngine;

namespace Platformer.AI
{
    public class AIGroundEnemyPatrolChaseAttack : MonoBehaviour
    {
        [SerializeField]
        private Transform leftPatrolPoint;

        [SerializeField]
        private Transform rightPatrolPoint;

        [SerializeField]
        private Transform attackRangeTransform;

        [SerializeField]
        private LayerMask attackableLayers;

        [SerializeField]
        private float chaseRange;

        [SerializeField]
        private float attackRange;

        [SerializeField]
        private float moveSpeed = 5f;

        [SerializeField]
        private float attackCooldownSeconds;

        private CharacterController2D characterController;
        private RaycastHit2D lineOfSightHit;
        private Transform target;
        private Transform playerTargetPoint;
        private Animator enemyAnimator;
        private Vector3 positionLastFrame;
        private Vector3 positionThisFrame;
        private float defaultMoveSpeed;
        private float distanceToTarget;
        private float attackCooldownTimer;
        private bool isAttacking;
        private bool canMove = true;
        private bool isInRange;
        private bool isInCooldown;
        private bool hasCustomSpeed = false;
        private bool isChasing = false;


        private bool isFacingLeft;
        public bool IsFacingLeft
        {
            get { return isFacingLeft; }
            set
            {
                if (isFacingLeft != value && canMove)
                {
                    if (isAttacking)
                        StartCoroutine(FlipDelayed(1f));
                    else
                        Flip();
                    isFacingLeft = value;
                }
            }
        }


        private void Awake()
        {
            defaultMoveSpeed = moveSpeed;
            SelectTarget();
            attackCooldownTimer = attackCooldownSeconds;
            enemyAnimator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController2D>();

            playerTargetPoint = GameObject.FindGameObjectWithTag("PlayerTargetPoint").transform;
        }

        private void Start()
        {
            positionLastFrame = transform.position;
        }

        private void Update()
        {
            positionThisFrame = transform.position;
            var velocity = (positionThisFrame - positionLastFrame) / Time.deltaTime;
            positionLastFrame = positionThisFrame;
            IsFacingLeft = velocity.x < 0;

            if (!isAttacking && canMove)
            {
                Move();
            }

            if (!InsideOfPatrollingPath() && !isInRange && !enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && canMove)
            {
                SelectTarget();
            }

            lineOfSightHit = Physics2D.Raycast(attackRangeTransform.position, transform.right, chaseRange, attackableLayers);

            distanceToTarget = Vector2.Distance(transform.position, target.position);
            RaycastDebugger();

            if (lineOfSightHit.collider && isInRange) //Player detected for attack
            {
                ProcessAttackBehaviour();
            }
            else
            {
                isInRange = false;
            }

            if (!isInRange)
            {
                InterruptAttacking();
            }

            if (isChasing)
            {
                moveSpeed = 1.5f * defaultMoveSpeed; ///TODO: test
            }
            else
            {
                moveSpeed = defaultMoveSpeed;
            }
        }


        public void ToggleCooldown(bool state = true)
        {
            isInCooldown = state;
        }

        public void ToggleMovingOn()
        {
            canMove = true;
        }

        public void ToggleMovingOff()
        {
            canMove = false;
        }


        private void ProcessAttackBehaviour()
        {

            if (distanceToTarget > attackRange)
            {
                InterruptAttacking();
            }
            else if (!isInCooldown)
            {
                Attack();
            }

            if (isInCooldown)
            {
                SetCooldown();
                enemyAnimator.ResetTrigger("Attack");
                ToggleMovingOn();
            }
        }

        private void Attack()
        {
            //attackCooldownSeconds = attackCooldownTimer; ///TODO: check if the player re-enters the range in less time than the cooldown time
            isAttacking = true;

            enemyAnimator.SetTrigger("Attack");
            enemyAnimator.SetBool("isRunning", false);
        }

        private void InterruptAttacking()
        {
            ToggleCooldown(false);
            isAttacking = false;
            enemyAnimator.ResetTrigger("Attack");
            ToggleMovingOn();
        }

        private void Move()
        {
            enemyAnimator.SetBool("isRunning", true);

            if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                var targetPosition = new Vector2(target.position.x, transform.position.y);

                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); ///TODO: Use CharacterController2D's move
            }
        }

        private void SelectTarget()
        {
            target = Vector3.Distance(transform.position, leftPatrolPoint.position) > Vector3.Distance(transform.position, rightPatrolPoint.position) ? leftPatrolPoint : rightPatrolPoint;
            StartCoroutine(FlipDelayed(3f));
        }

        private bool InsideOfPatrollingPath()
        {
            return transform.position.x > leftPatrolPoint.position.x && transform.position.x < rightPatrolPoint.position.x;
        }

        private void Flip()
        {
            var rotation = transform.eulerAngles;
            rotation.y = transform.position.x < target.position.x ? 180f : 0f;
            transform.eulerAngles = rotation;
        }

        private IEnumerator FlipDelayed(float delay)
        {
            hasCustomSpeed = true;
            moveSpeed = 0;
            enemyAnimator.SetTrigger("Idle");
            yield return new WaitForSeconds(delay);
            Flip();
            hasCustomSpeed = false;
        }

        private void SetCooldown()
        {
            attackCooldownSeconds -= Time.deltaTime;

            if (attackCooldownSeconds <= 0 && isInCooldown && isAttacking)
            {
                ToggleCooldown(false);
                attackCooldownSeconds = attackCooldownTimer;
            }
        }


        private void RaycastDebugger()
        {
            if (distanceToTarget > attackRange)
            {
                Debug.DrawRay(attackRangeTransform.position, -transform.right * chaseRange, Color.red);
            }
            else
            {
                Debug.DrawRay(attackRangeTransform.position, -transform.right * chaseRange, Color.green);
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (playerTargetPoint)
                    target = playerTargetPoint;
                else
                    target = collision.transform;
                isInRange = true;
                isChasing = true; ///TODO: another animation or particles/effects while chasing
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                isChasing = false; ///TODO: stop animation/particles/effects for chasing
            }
        }
    }
}
