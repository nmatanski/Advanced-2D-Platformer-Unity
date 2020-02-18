using System;
using UnityEngine;

namespace Platformer.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; set; }

        [SerializeField]
        private Sound[] sounds;

        [SerializeField]
        private GameObject audioSourcesGameObject;


        // Start is called before the first frame update
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            foreach (var sound in sounds)
            {
                sound.Source = audioSourcesGameObject.AddComponent<AudioSource>();

                sound.Source.clip = sound.Clip;
                sound.Source.volume = sound.Volume;
                sound.Source.pitch = sound.Pitch;
                sound.Source.spatialBlend = sound.SpatialBlend;
                sound.Source.loop = sound.IsLooping;
            }
        }

        private void Start()
        {
            //Play(GlobalData.AudioSources.MainTheme);
        }


        /// <param name="name">Use GlobalData.AudioSources</param>
        public void Play(string name)
        {
            var soundSource = Array.Find(sounds, sound => sound.Name == name)?.Source; ///TODO: null check instead + Debug.LogWarning
            soundSource?.Play();
        }

        /// <param name="names">Use GlobalData.AudioSources</param>
        /// <param name="isRandomized">true: play 1 of the array; false: play all of them</param>
        public void Play(bool isRandomized = false, params string[] names)
        {
            if (isRandomized)
            {
                var name = names[UnityEngine.Random.Range(0, names.Length)];
                var soundSource = Array.Find(sounds, sound => sound.Name == name)?.Source;  ///TODO: null check instead + Debug.LogWarning
                soundSource?.Play();

                return;
            }

            foreach (var name in names)
            {
                var soundSource = Array.Find(sounds, sound => sound.Name == name)?.Source;  ///TODO: null check instead + Debug.LogWarning
                soundSource?.Play();
            }
        }
    }
}
