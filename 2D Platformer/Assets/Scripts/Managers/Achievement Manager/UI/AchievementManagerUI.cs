using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Achievements.UI
{
    public class AchievementManagerUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject achievementPrefab;

        [SerializeField]
        private Sprite[] sprites;

        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private GameObject achievementMenu;


        private AchievementButtonUI activeButton;


        // Start is called before the first frame update
        private void Start()
        {
            activeButton = GameObject.FindGameObjectWithTag("GeneralButtonUI").GetComponent<AchievementButtonUI>();

            CreateAchievementUI("General", "General Achievement 1", "descr", 5, 0);
            CreateAchievementUI("General", "General Achievement 2", "descr", 5, 0);
            CreateAchievementUI("General", "General Achievement 3", "descr", 5, 0);
            CreateAchievementUI("General", "General Achievement 4", "descr", 5, 0);
            CreateAchievementUI("General", "General Achievement 5", "descr", 5, 0);
            CreateAchievementUI("General", "General Achievement 6", "descr", 5, 0);
            CreateAchievementUI("General", "General Achievement 7", "descr", 5, 0);
            CreateAchievementUI("General", "General Achievement 8", "descr", 5, 0);
            CreateAchievementUI("General", "General Achievement 9", "descr", 5, 0);
            CreateAchievementUI("General", "General Achievement 10", "descr", 5, 0);
            CreateAchievementUI("General", "General Achievement 11", "descr", 5, 0);

            CreateAchievementUI("Other", "Other Achievement 1", "descr", 5, 0);
            CreateAchievementUI("Other", "Other Achievement 2", "descr", 5, 0);
            CreateAchievementUI("Other", "Other Achievement 3", "descr", 5, 0);
            CreateAchievementUI("Other", "Other Achievement 4", "descr", 5, 0);
            CreateAchievementUI("Other", "Other Achievement 5", "descr", 5, 0);
            CreateAchievementUI("Other", "Other Achievement 6", "descr", 5, 0);
            CreateAchievementUI("Other", "Other Achievement 7", "descr", 5, 0);
            CreateAchievementUI("Other", "Other Achievement 8", "descr", 5, 0);
            CreateAchievementUI("Other", "Other Achievement 9", "descr", 5, 0);
            CreateAchievementUI("Other", "Other Achievement 10", "descr", 5, 0);
            CreateAchievementUI("Other", "Other Achievement 11", "descr", 5, 0);


            foreach (var achievementCategory in GameObject.FindGameObjectsWithTag("AchievementCategoryUI"))
            {
                achievementCategory.SetActive(false);
            }

            activeButton.OnClick();

            achievementMenu.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                achievementMenu.SetActive(!achievementMenu.activeSelf);
            }
        }


        public void ChangeCategory(GameObject button)
        {
            var achievementButton = button.GetComponent<AchievementButtonUI>();

            scrollRect.content = achievementButton.AchievementCategory.GetComponent<RectTransform>();

            achievementButton.OnClick();
            activeButton.OnClick();
            activeButton = achievementButton;
        }


        private void CreateAchievementUI(string category, string title, string description, int points, int spriteIndex)
        {
            var achievement = Instantiate(achievementPrefab);
            SetAchievementInfoUI(category, achievement, title, description, points, spriteIndex);
        }

        private void SetAchievementInfoUI(string category, GameObject achievement, string title, string description, int points, int spriteIndex)
        {
            achievement.transform.SetParent(GameObject.Find(category).transform);
            achievement.transform.localScale = Vector3.one;
            achievement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
            achievement.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
            achievement.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = points.ToString();
            achievement.transform.GetChild(3).GetComponent<Image>().sprite = sprites[spriteIndex];
        }
    }
}
