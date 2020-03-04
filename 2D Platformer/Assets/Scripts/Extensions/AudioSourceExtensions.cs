using System.Collections;
using UnityEngine;

namespace Platformer.Extensions
{
    public static class AudioSourceExtensions
    {
        private static bool isFadingOutThis = false;

        public static void FadeOut(this AudioSource audioSource, float duration, out bool isFadingOut)
        {
            isFadingOut = true;

            new MonoBehaviour().StartCoroutine(StartFadeOut(audioSource, duration));
            while (true)
            {
                if (!isFadingOutThis)
                {
                    isFadingOut = false;
                    break;
                }
            }
        }

        private static IEnumerator StartFadeOut(AudioSource audioSource, float duration)
        {
            isFadingOutThis = true;

            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
                yield return null;
            }

            isFadingOutThis = false;
        }
    }
}
