using Platformer.Audio;
using Platformer.Player;
using System.Collections;
using UnityEngine;

namespace Platformer.Interactables
{
    public class MinableObject : MonoBehaviour
    {
        [SerializeField]
        private int HealthClickCount = 3;

        [SerializeField]
        private GameObject pickup;

        [SerializeField]
        private int thicknessMultiplier = 2;

        private SelectableObject selectable;
        private float defaultOutlineThickness;
        private bool isPlayerInRange = false;
        private AudioManager audioManager;


        private void Start()
        {
            selectable = GetComponent<SelectableObject>();
            defaultOutlineThickness = selectable.OutlineThickness;

            audioManager = FindObjectOfType<AudioManager>();
        }

        private void Update()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider && hit.collider.gameObject == gameObject && Input.GetMouseButtonDown(1) && isPlayerInRange)
            {
                StartCoroutine(IncreaseThickness(thicknessMultiplier, .2f));

                ///TODO: Sound effect of mining - tests required (randomness, scene changing)
                audioManager.Play(true, GlobalData.AudioSources.CrystalMining1_wav, GlobalData.AudioSources.CrystalMining2_wav, GlobalData.AudioSources.CrystalMining3_wav);

                HealthClickCount = Mathf.Clamp(HealthClickCount - 1, 0, int.MaxValue);

                if (HealthClickCount < 1)
                {
                    if (pickup != null)
                    {
                        Instantiate(pickup);
                    }

                    Destroy(gameObject);
                }
            }
        }


        private IEnumerator IncreaseThickness(int thicknessMultiplier, float duration)
        {
            selectable.OutlineThickness = 0f;
            yield return new WaitForSeconds(duration / 2f);
            selectable.OutlineThickness *= thicknessMultiplier;
            yield return new WaitForSeconds(duration / 2f);
            selectable.OutlineThickness = defaultOutlineThickness;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "PlayerMiningRange")
            {
                isPlayerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "PlayerMiningRange")
            {
                isPlayerInRange = false;
            }
        }
    }
}
