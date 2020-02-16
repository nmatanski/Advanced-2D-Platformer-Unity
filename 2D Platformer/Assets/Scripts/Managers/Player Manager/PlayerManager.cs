using Anima2D;
using Platformer.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Managers
{
    public class PlayerManager : MonoBehaviour, IManager
    {
        public ManagerStatus Status { get; private set; }
        public Vector3 LastAttackDirection { get; set; }

        [SerializeField]
        private int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        [SerializeField]
        private int maxHealth = 100;
        public int MaxHealth
        {
            get { return maxHealth; }
            private set { maxHealth = value; }
        }


        [SerializeField]
        private int gold;
        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }

        [SerializeField]
        private bool isDead = false;
        public bool IsDead
        {
            get { return isDead; }
            private set { isDead = value; }
        }

        [SerializeField]
        private bool isInvulnerable;
        public bool IsInvulnerable
        {
            get { return isInvulnerable; }
            private set { isInvulnerable = value; }
        }

        [SerializeField]
        private float invulnerabilityDuration = 1f;

        [SerializeField]
        private Color invulnerabilityColorAndAlpha = new Color(1f, 1f, 1f, 0.25f);

        [SerializeField]
        private int invulnerabilityFlickerCount = 10;

        [SerializeField]
        private GameObject deathEffect;
        public GameObject DeathEffect
        {
            get { return deathEffect; }
            private set { deathEffect = value; }
        }

        [SerializeField]
        private float respawnDelay = 3f;

        [SerializeField]
        private SelectedObject lastSelectedObject;
        public SelectedObject LastSelectedObject
        {
            get { return lastSelectedObject; }
            set { lastSelectedObject = value; }
        }


        private PlayerController playerController;
        private SpriteMeshInstance[] playerSprites;
        private GameObject playerGO;
        private Rigidbody2D playerRigidBody;
        private BoxCollider2D playerCollider;
        private Scene scene;
        private InventoryManager inventoryManager;
        private Managers managers;


        public void Startup()
        {
            Debug.Log("Player Manager is starting...");

            Health = MaxHealth;
            //Health = 100; ///TODO: load this from savefile
            //MaxHealth = 100; ///TODO: load this from savefile

            Status = ManagerStatus.Initializing;

            ///long-runing startups tasks here, set Status to Initializing 

            StartCoroutine(LoadAndCache());

            Debug.Log(Status);
        }

        private void Update()
        {
            if (!managers.AreReady)
            {
                Debug.LogWarning("Managers not ready yet.");
                return;
            }

            if (!LastSelectedObject.HasSelectedObject)
            {
                LastSelectedObject.SelectedGameObject = null;
            }

            //if (Input.anyKey) // key down for heal?
            //{
            //    inventoryManager.UseItem(item?); ///TODO: check if UseItem is returning true or false
            //}
        }


        /// <param name="value">Could be both positive and negative value</param>
        public void AddHealth(short value)
        {
            if (value < 0) // taking damage
            {
                if (!IsInvulnerable)
                {
                    Health = Mathf.Clamp(Health + value, 0, MaxHealth);
                }

                if (Health == 0 && !IsDead)
                {
                    IsDead = true;
                    KillPlayer();
                }
                else if (Health != 0)
                {
                    ///TODO: Play sound of taking damage
                    StartCoroutine(MakeInvulnerable(invulnerabilityDuration, invulnerabilityFlickerCount));
                }
            }
            else if (value > 0) // healing
            {
                Health = Mathf.Clamp(Health + value, 0, MaxHealth);
                ///TODO: Play sound of healing
                ///TODO: Play healing effect
            }

            Debug.Log($"Health: {Health}/{MaxHealth}");
        }

        /// <param name="value">Could be both positive and negative value</param>
        public void AddGold(int value)
        {
            Gold = Mathf.Clamp(Gold + value, 0, int.MaxValue);

            Debug.Log($"Gold: {Gold}");
        }

        public void KillPlayer()
        {
            ///TODO: player death effect
            foreach (var sprite in playerSprites)
            {
                sprite.enabled = false;
            }
            playerController.enabled = false;
            playerCollider.enabled = false;

            StartCoroutine(RestartLevel(respawnDelay));
        }

        private void ChangeSpritesColor(SpriteMeshInstance[] sprites, Color color)
        {
            foreach (var sprite in sprites)
            {
                sprite.color = color;
            }
        }

        private IEnumerator MakeInvulnerable(float invulnerabilityDuration, int flickerCount = 10)
        {
            IsInvulnerable = true;

            ///TODO: make better knockback effect
            playerRigidBody.MovePosition(playerRigidBody.position + new Vector2(Mathf.Sign(LastAttackDirection.x) * .25f, .125f));

            for (int i = 0; i < flickerCount; i++)
            {
                ChangeSpritesColor(playerSprites, invulnerabilityColorAndAlpha);
                yield return new WaitForSeconds(invulnerabilityDuration / (2 * flickerCount));
                ChangeSpritesColor(playerSprites, Color.white); ///TODO: get the original color of the sprites and remove it from the Startup
                yield return new WaitForSeconds(invulnerabilityDuration / (2 * flickerCount));
            }

            IsInvulnerable = false;
        }

        private IEnumerator RestartLevel(float respawnDelay)
        {
            ///TODO: death screen or transition for restarting the level
            yield return new WaitForSeconds(respawnDelay);
            SceneManager.LoadScene(scene.name);
            IsDead = false;
            health = MaxHealth;
        }

        public IEnumerator LoadAndCache()
        {
            yield return StartCoroutine(Load());

            Status = ManagerStatus.Started;
            Debug.Log(Status);
        }

        public IEnumerator Load()
        {
            managers = GetComponent<Managers>();
            inventoryManager = InventoryManager.Instance; ///TODO: more testing required: gets null but it loads it through the reference later

            playerGO = GameObject.FindGameObjectWithTag("Player");
            playerController = playerGO.GetComponent<PlayerController>();
            playerCollider = playerGO.GetComponent<BoxCollider2D>();
            scene = SceneManager.GetActiveScene();
            playerSprites = playerGO.GetComponentsInChildren<SpriteMeshInstance>();
            ChangeSpritesColor(playerSprites, Color.white); //to reset the original sprites' colors
            playerRigidBody = playerGO.GetComponent<Rigidbody2D>();
            managers = GetComponent<Managers>();

            yield return null;
        }
    }
}
