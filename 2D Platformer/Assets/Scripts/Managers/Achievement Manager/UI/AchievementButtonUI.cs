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

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

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
