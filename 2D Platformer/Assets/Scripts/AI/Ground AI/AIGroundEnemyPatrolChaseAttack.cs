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
        private float moveSpeed;

        [SerializeField]
        private float attackCooldownSeconds;

        private CharacterController2D characterController;
        private RaycastHit2D lineOfSightHit;
        private Transform target;
        private Animator enemyAnimator;
        private Vector3 positionLastFrame;
        private Vector3 positionThisFrame;
        private float distanceToTarget;
        private float attackCooldownTimer;
        private bool isAttacking;
        private bool isInRange;
        private bool isInCooldown;


        private bool isFacingLeft;
        public bool IsFacingLeft
        {
            get { return isFacingLeft; }
            set
            {
                if (isFacingLeft != value)
                    if (isAttacking)
                        StartCoroutine(FlipDelayed(.5f));
                    else
                        Flip();
                isFacingLeft = value;
            }
        }


        private void Awake()
        {
            SelectTarget();
            attackCooldownTimer = attackCooldownSeconds;
            enemyAnimator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController2D>();
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

            if (!isAttacking)
            {
                Move();
            }

            if (!InsideOfPatrollingPath() && !isInRange && !enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) ///remove last check?
            {
                SelectTarget();
            }

            if (isInRange)
            {
                lineOfSightHit = Physics2D.Raycast(attackRangeTransform.position, transform.right, chaseRange, attackableLayers);
            }

            distanceToTarget = Vector2.Distance(transform.position, target.position);
            RaycastDebugger();


            if (lineOfSightHit.collider) //Player detected
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
        }


        public void ToggleCooldown(bool state = true)
        {
            isInCooldown = state;
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
            }
        }

        private void Attack()
        {
            attackCooldownSeconds = attackCooldownTimer; ///TODO: check if the player re-enters the range in less time than the cooldown time
            isAttacking = true;

            enemyAnimator.SetTrigger("Attack");
            enemyAnimator.SetBool("isRunning", false);
        }

        private void InterruptAttacking()
        {
            ToggleCooldown(false);///TODO: Wait for the timer to be = 0
            isAttacking = false;
            //enemyAnimator.SetBool("Attack", false);
            enemyAnimator.ResetTrigger("Attack");
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
            Flip();
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
            yield return new WaitForSeconds(delay);
            Flip();
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
                target = collision.transform;
                isInRange = true;
                Flip();
            }
        }
    }
}
