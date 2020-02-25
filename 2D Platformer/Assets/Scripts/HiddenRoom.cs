using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Platformer
{
    public class HiddenRoom : MonoBehaviour
    {
        [SerializeField]
        private bool isExplored = false;

        private TilemapRenderer tilemapRenderer;
        private Tilemap tilemap;
        //private AudioManager audioManager;


        void Start()
        {
            //audioManager = FindObjectOfType<AudioManager>();
            tilemapRenderer = GetComponent<TilemapRenderer>();
            tilemap = GetComponent<Tilemap>();

            if (isExplored)
            {
                var color = tilemap.color;
                tilemap.color = new Color(color.r, color.g, color.b, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && !isExplored)
            {
                ///TODO: Explore secret room SFX
                //AudioManager.Instance.Play(true, true, GlobalData.AudioSources.CrystalMining1_wav, GlobalData.AudioSources.CrystalMining2_wav, GlobalData.AudioSources.CrystalMining3_wav); ///TODO: test sounds, change them later
                Managers.Managers.AudioManager.Play(true, true, GlobalData.AudioSources.CrystalMining1_wav, GlobalData.AudioSources.CrystalMining2_wav, GlobalData.AudioSources.CrystalMining3_wav); ///TODO: test sounds, change them later
                ///TODO Managers.AudioManager or AudioManager.Instance

                StartCoroutine(FadeTo(tilemap, 0, .3f));
            }
        }

        private IEnumerator FadeTo(Tilemap tilemap, float aValue, float aTime)
        {
            var color = tilemap.color;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                tilemap.color = new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, aValue, t));
                yield return null;
            }

            tilemapRenderer.enabled = false;
            isExplored = true;
        }
    }
}
