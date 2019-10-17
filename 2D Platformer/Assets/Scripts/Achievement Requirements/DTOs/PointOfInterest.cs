using UnityEngine;

namespace Platformer.Achievements
{
    public class PointOfInterest : ScriptableObject
    {
        [SerializeField]
        private string title;
        public string Title { get { return title; } }

        [SerializeField]
        private string description;
        public string Description { get { return description; } }
    }
}
