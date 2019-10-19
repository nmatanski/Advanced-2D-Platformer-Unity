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
        private Sprite neutral;

        [SerializeField]
        private Sprite highlight;


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
