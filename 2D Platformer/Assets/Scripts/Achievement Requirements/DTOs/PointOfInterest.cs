using UnityEngine;

namespace Platformer.Achievements
{
    [CreateAssetMenu(fileName = "Achievement Data")]
    public class PointOfInterest : ScriptableObject
    {
        [SerializeField]
        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }


        [SerializeField]
        private string name;
        public string Name { get { return name; } }

        [SerializeField]
        private string description;
        public string Description { get { return description; } }
    }
}
