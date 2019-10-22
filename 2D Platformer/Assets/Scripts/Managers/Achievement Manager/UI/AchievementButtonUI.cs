using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Achievements.UI
{
    public class AchievementButtonUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject achievementCategory;
        public GameObject AchievementCategory
        {
            get { return achievementCategory; }
            set { achievementCategory = value; }
        }

        [SerializeField]
#pragma warning disable CS0649 // Field 'AchievementButtonUI.neutral' is never assigned to, and will always have its default value null
        private Sprite neutral;
#pragma warning restore CS0649 // Field 'AchievementButtonUI.neutral' is never assigned to, and will always have its default value null

        [SerializeField]
#pragma warning disable CS0649 // Field 'AchievementButtonUI.highlight' is never assigned to, and will always have its default value null
        private Sprite highlight;
#pragma warning restore CS0649 // Field 'AchievementButtonUI.highlight' is never assigned to, and will always have its default value null


        private Image sprite;


        private void Awake()
        {
            sprite = GetComponent<Image>();
        }


        public void OnClick()
        {
            if (sprite.sprite == neutral)
            {
                sprite.sprite = highlight;
                AchievementCategory.SetActive(true);
            }
            else
            {
                sprite.sprite = neutral;
                AchievementCategory.SetActive(false);
            }
        }
    }
}
