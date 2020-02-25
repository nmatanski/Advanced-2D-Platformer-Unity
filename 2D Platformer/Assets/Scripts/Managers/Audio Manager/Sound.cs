using System;
using UnityEngine;

namespace Platformer.Audio
{
    [Serializable]
    public class Sound
    {
        [SerializeField]
        private string name;
        public string Name { get => name; set => name = value; }

        [SerializeField]
        private AudioClip clip;
        public AudioClip Clip { get => clip; set => clip = value; }

        [SerializeField]
        [Range(0, 1)]
        private float volume = 1;
        public float Volume { get => volume; set => volume = value; }

        [SerializeField]
        [Range(.1f, 3)]
        private float pitch = 1;
        public float Pitch { get => pitch; set => pitch = value; }

        [SerializeField]
        [Range(0, 1)]
        private float spatialBlend = 1;
        public float SpatialBlend { get => spatialBlend; set => spatialBlend = value; }

        [SerializeField]
        private bool isLooping;
        public bool IsLooping { get => isLooping; set => isLooping = value; }


        public AudioSource Source { get; set; }
    }
}
