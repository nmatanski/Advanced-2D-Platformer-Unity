using Platformer.Audio;
using Platformer.Extensions;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer.Managers
{
    public class AudioManager : MonoBehaviour, IManager
    {
        public static AudioManager Instance { get; private set; }

        public ManagerStatus Status { get; private set; }


        [SerializeField]
        private Sound[] sounds;

        [SerializeField]
        private GameObject audioSourcesGameObject;

        [SerializeField]
        private Sound currentThemeSong;

        private Sound previousThemeSong;
        private bool isThemeSongFadingOut = false;


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


            ///long-runing startups tasks here, set Status to Initializing 

            StartCoroutine(LoadAndCache());

            Debug.Log(Status);
        }


        /// <param name="name">Use GlobalData.AudioSources</param>
        public Sound Play(string name, bool isReloadingSettings = true)
        {
            var sound = Array.Find(sounds, s => s.Name == name);
            //var soundSource = sound?.Source; ///TODO: null check instead + Debug.LogWarning
            if (isReloadingSettings)
            {
                SetSound(sound);
            }

            sound?.Source?.Play();

            return sound;
        }

        /// <param name="names">Use GlobalData.AudioSources</param>
        /// <param name="isRandomized">true: play 1 of the array; false: play all of them</param>
        public Sound Play(bool isRandomized = false, bool isReloadingSettings = true, params string[] names)
        {
            if (isRandomized)
            {
                var name = names[UnityEngine.Random.Range(0, names.Length)];
                var sound = Array.Find(sounds, s => s.Name == name);
                //var soundSource = sound?.Source; ///TODO: null check instead + Debug.LogWarning
                if (isReloadingSettings)
                {
                    SetSound(sound);
                }

                sound?.Source?.Play();

                return sound;
            }

            Sound lastSound = null;
            foreach (var name in names)
            {
                var currentSound = Array.Find(sounds, sound => sound.Name == name);
                var soundSource = currentSound?.Source;  ///TODO: null check instead + Debug.LogWarning
                soundSource?.Play();

                lastSound = currentSound;
            }

            return lastSound;
        }

        public void ChangeMainTheme(Sound themeSong)
        {
            if (currentThemeSong.Name.Equals(themeSong.Name))
            {
                Debug.LogWarning("The theme song is the same, it hasn't been changed.");
                return;
            }

            isThemeSongFadingOut = true;
            currentThemeSong.Source.FadeOut(3.5f, out isThemeSongFadingOut); ///TODO: Test the extension if working
            previousThemeSong = new Sound(currentThemeSong);
            //previousThemeSong.Source.FadeOut(3.5f); ///TODO: Test the extension if working

            var newAudioSource = GetAudioSource(themeSong);
            themeSong.Source = newAudioSource; //second separate source
            StartCoroutine(PlaySoundDelayed(themeSong));
        }


        private void PlayMainTheme()
        {
            ///TODO: Add a main theme
            currentThemeSong = Play(GlobalData.AudioSources.AIMinorLoop_wav_3d);
        }

        private static void SetSound(Sound sound)
        {
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.spatialBlend = sound.SpatialBlend;
            sound.Source.loop = sound.IsLooping;
        }

        private AudioSource GetAudioSource(Sound sound)
        {
            return sound.LocationGameObject ? sound.LocationGameObject.AddComponent<AudioSource>() : audioSourcesGameObject.AddComponent<AudioSource>();
        }


        private IEnumerator PlaySoundDelayed(Sound themeSong)
        {
            while (isThemeSongFadingOut)
            {
                yield return null;
            }

            currentThemeSong = Play(themeSong.Name); ///TODO: add fade in in the Play method with bool hasFadeIn = true and a fadein method called in Play() - both methods
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
                sound.Source = GetAudioSource(sound);

                SetSound(sound);
            }

            PlayMainTheme();

            yield return null;
        }
    }
}
