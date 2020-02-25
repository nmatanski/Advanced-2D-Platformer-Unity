using Platformer.Audio;
using Platformer.Managers;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer.Managers
{
    public class AudioManager : MonoBehaviour, IManager
    {
        public static AudioManager Instance { get; set; }

        public ManagerStatus Status { get; private set; }


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
        }


        public void Startup()
        {
            Debug.Log("Audio Manager is starting...");

            Status = ManagerStatus.Initializing;

            //Play(GlobalData.AudioSources.MainTheme);

            ///long-runing startups tasks here, set Status to Initializing 

            StartCoroutine(LoadAndCache());

            Debug.Log(Status);
        }

        public IEnumerator LoadAndCache()
        {
            yield return StartCoroutine(Load());

            Status = ManagerStatus.Started;
            Debug.Log(Status);
        }

        public IEnumerator Load()
        {
            foreach (var sound in sounds)
            {
                sound.Source = audioSourcesGameObject.AddComponent<AudioSource>();

                sound.Source.clip = sound.Clip;
                sound.Source.volume = sound.Volume;
                sound.Source.pitch = sound.Pitch;
                sound.Source.spatialBlend = sound.SpatialBlend;
                sound.Source.loop = sound.IsLooping;
            }

            yield return null;
        }


        /// <param name="name">Use GlobalData.AudioSources</param>
        public void Play(string name, bool isReloadingSettings = true)
        {
            var sound = Array.Find(sounds, s => s.Name == name);
            var soundSource = sound?.Source; ///TODO: null check instead + Debug.LogWarning
            if (isReloadingSettings)
            {
                soundSource.clip = sound.Clip;
                soundSource.volume = sound.Volume;
                soundSource.pitch = sound.Pitch;
                soundSource.spatialBlend = sound.SpatialBlend;
                soundSource.loop = sound.IsLooping;
            }

            soundSource?.Play();
        }

        /// <param name="names">Use GlobalData.AudioSources</param>
        /// <param name="isRandomized">true: play 1 of the array; false: play all of them</param>
        public void Play(bool isRandomized = false, bool isReloadingSettings = true, params string[] names)
        {
            if (isRandomized)
            {
                var name = names[UnityEngine.Random.Range(0, names.Length)];
                var sound = Array.Find(sounds, s => s.Name == name);
                var soundSource = sound?.Source; ///TODO: null check instead + Debug.LogWarning
                if (isReloadingSettings)
                {
                    soundSource.clip = sound.Clip;
                    soundSource.volume = sound.Volume;
                    soundSource.pitch = sound.Pitch;
                    soundSource.spatialBlend = sound.SpatialBlend;
                    soundSource.loop = sound.IsLooping;
                }

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
