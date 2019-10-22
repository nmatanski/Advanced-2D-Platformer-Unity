using UnityEngine;

namespace Platformer.Achievements
{
    [CreateAssetMenu(fileName = "Achievement Data")]
    public class PointOfInterest : ScriptableObject
    {
        [SerializeField]
#pragma warning disable CS0649 // Field 'PointOfInterest.id' is never assigned to, and will always have its default value null
        private string id;
#pragma warning restore CS0649 // Field 'PointOfInterest.id' is never assigned to, and will always have its default value null
        public string ID { get { return id; } }

        [SerializeField]
#pragma warning disable CS0649 // Field 'PointOfInterest.category' is never assigned to, and will always have its default value null
        private string category;
#pragma warning restore CS0649 // Field 'PointOfInterest.category' is never assigned to, and will always have its default value null

        public string Category { get { return category; } }

        [SerializeField]
#pragma warning disable CS0108 // 'PointOfInterest.name' hides inherited member 'Object.name'. Use the new keyword if hiding was intended.
#pragma warning disable CS0649 // Field 'PointOfInterest.name' is never assigned to, and will always have its default value null
        private string name;
#pragma warning restore CS0649 // Field 'PointOfInterest.name' is never assigned to, and will always have its default value null
#pragma warning restore CS0108 // 'PointOfInterest.name' hides inherited member 'Object.name'. Use the new keyword if hiding was intended.
        public string Name { get { return name; } }

        [SerializeField]
#pragma warning disable CS0649 // Field 'PointOfInterest.description' is never assigned to, and will always have its default value null
        private string description;
#pragma warning restore CS0649 // Field 'PointOfInterest.description' is never assigned to, and will always have its default value null
        public string Description { get { return description; } }

        [SerializeField]
#pragma warning disable CS0649 // Field 'PointOfInterest.points' is never assigned to, and will always have its default value 0
        private int points;
#pragma warning restore CS0649 // Field 'PointOfInterest.points' is never assigned to, and will always have its default value 0
        public int Points { get { return points; } }

        [SerializeField]
#pragma warning disable CS0649 // Field 'PointOfInterest.sprite' is never assigned to, and will always have its default value null
        private Sprite sprite;
#pragma warning restore CS0649 // Field 'PointOfInterest.sprite' is never assigned to, and will always have its default value null
        public Sprite Sprite { get { return sprite; } }
    }
}
