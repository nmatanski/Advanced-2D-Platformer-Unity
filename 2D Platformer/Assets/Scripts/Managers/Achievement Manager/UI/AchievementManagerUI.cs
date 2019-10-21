using System.Collections;
using System.Collections.Generic;
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
        public GameObject AchievementMenu { get => achievementMenu; private set => achievementMenu = value; }

        [SerializeField]
        private GameObject visualAchievement;

        [SerializeField]
        private Sprite unlockedAchievementSprite;
        public Sprite UnlockedAchievementSprite { get => unlockedAchievementSprite; private set => unlockedAchievementSprite = value; }

        [SerializeField]
        private TextMeshProUGUI pointsText;
        public TextMeshProUGUI PointsText { get => pointsText; private set => pointsText = value; }


        public Dictionary<string, Achievement> Achievements { get; private set; } = new Dictionary<string, Achievement>();

        private static AchievementManagerUI instance;
        public static AchievementManagerUI Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AchievementManagerUI>();
                }
                return instance;
            }
        }

        private AchievementButtonUI activeButton;


        // Start is called before the first frame update
        private void Start()
        {
            activeButton = GameObject.FindGameObjectWithTag("GeneralButtonUI").GetComponent<AchievementButtonUI>();

            CreateAchievementUI("General", "There are achievements in this game?", "Open the achievements.", 5, 0); ///TODO: serializefield and use the gameobject with the requirement
            CreateAchievementUI("General", "Jump", "Find one of the jump keys.", 5, 0);
            CreateAchievementUI("General", "I can walk!", "Find how to walk.", 5, 0);
            //CreateAchievementUI("General", "Full control", "Learn how to walk and jump.", 10, 0, new string[] { "Jump", "I can walk!" });

            CreateAchievementUI("Other", "I need to die now!? Please!", "Find the special ending.", 5, 0);
            CreateAchievementUI("Other", "You're not supposed to be here!", "Explore the special place.", 5, 0);

            foreach (var achievementCategory in GameObject.FindGameObjectsWithTag("AchievementCategoryUI"))
            {
                achievementCategory.SetActive(false);
            }

            activeButton.OnClick();

            AchievementMenu.SetActive(false);
        }


        public void ChangeCategory(GameObject button)
        {
            var achievementButton = button.GetComponent<AchievementButtonUI>();

            scrollRect.content = achievementButton.AchievementCategory.GetComponent<RectTransform>();

            achievementButton.OnClick();
            activeButton.OnClick();
            activeButton = achievementButton;
        }

        public void TryEarnAchievement(string title, bool isUnlocked, string key)
        {
            Debug.Log('\t' + title);
            if (!Achievements[title].IsEarned(isUnlocked, key) && !isUnlocked)
            {
                var achievement = Instantiate(visualAchievement);
                SetAchievementInfoUI("EarnAchievementCanvas", achievement, title);
                StartCoroutine(HideAchievement(achievement, 3f));
            }
        }


        public IEnumerator HideAchievement(GameObject achievement, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(achievement);
        }


        private void CreateAchievementUI(string parent, string title, string description, int points, int spriteIndex, string[] achievementRequirementsTitles = null)
        {
            var achievement = Instantiate(achievementPrefab);
            var newAchievement = new Achievement(title, description, points, spriteIndex, achievement);
            Achievements.Add(title, newAchievement);
            SetAchievementInfoUI(parent, achievement, title);

            if (achievementRequirementsTitles != null)
            {
                foreach (var requiredAchievementTitle in achievementRequirementsTitles)
                {
                    var dependency = Achievements[requiredAchievementTitle];
                    dependency.ChildAchievement = title;
                    newAchievement.AddRequiredAchievement(dependency);
                }
            }
        }

        private void SetAchievementInfoUI(string parent, GameObject achievement, string title)
        {
            achievement.transform.SetParent(GameObject.Find(parent).transform);
            achievement.transform.localScale = Vector3.one;
            achievement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
            achievement.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Achievements[title].Description;
            achievement.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Achievements[title].Points.ToString();
            achievement.transform.GetChild(3).GetComponent<Image>().sprite = sprites[Achievements[title].SpriteIndex];
        }
    }
}
