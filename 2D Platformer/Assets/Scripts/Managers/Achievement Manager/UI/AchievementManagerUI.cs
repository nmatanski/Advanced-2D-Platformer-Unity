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
#pragma warning disable CS0649 // Field 'AchievementManagerUI.achievementPrefab' is never assigned to, and will always have its default value null
        private GameObject achievementPrefab;
#pragma warning restore CS0649 // Field 'AchievementManagerUI.achievementPrefab' is never assigned to, and will always have its default value null

        [SerializeField]
#pragma warning disable CS0649 // Field 'AchievementManagerUI.sprites' is never assigned to, and will always have its default value null
        private Sprite[] sprites;
#pragma warning restore CS0649 // Field 'AchievementManagerUI.sprites' is never assigned to, and will always have its default value null

        [SerializeField]
#pragma warning disable CS0649 // Field 'AchievementManagerUI.scrollRect' is never assigned to, and will always have its default value null
        private ScrollRect scrollRect;
#pragma warning restore CS0649 // Field 'AchievementManagerUI.scrollRect' is never assigned to, and will always have its default value null

        [SerializeField]
        private GameObject achievementMenu;
        public GameObject AchievementMenu { get => achievementMenu; private set => achievementMenu = value; }

        [SerializeField]
#pragma warning disable CS0649 // Field 'AchievementManagerUI.visualAchievement' is never assigned to, and will always have its default value null
        private GameObject visualAchievement;
#pragma warning restore CS0649 // Field 'AchievementManagerUI.visualAchievement' is never assigned to, and will always have its default value null

        [SerializeField]
        private Sprite unlockedAchievementSprite;
        public Sprite UnlockedAchievementSprite { get => unlockedAchievementSprite; private set => unlockedAchievementSprite = value; }

        [SerializeField]
        private TextMeshProUGUI pointsText;
        public TextMeshProUGUI PointsText { get => pointsText; private set => pointsText = value; }

        [SerializeField]
        private List<PointOfInterest> achievementDTOs = new List<PointOfInterest>();

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

            foreach (var achievement in achievementDTOs)
            {
                CreateAchievementUI(achievement.Category, achievement.Name, achievement.Description, achievement.Points);
            }

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
                StartCoroutine(FadeAchievementNotification(achievement, 1f, 3f, 2f));
            }
        }


        public IEnumerator HideAchievement(GameObject achievement, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(achievement);
        }


        private void CreateAchievementUI(string parent, string title, string description, int points, int spriteIndex = 0, string[] achievementRequirementsTitles = null)
        {
            var achievement = Instantiate(achievementPrefab);
            var newAchievement = new Achievement(title, description, points, spriteIndex, achievement); ///TODO: change spriteIndex for Sprite
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

        private IEnumerator FadeAchievementNotification(GameObject achievement, float fadeInTime, float downtime, float fadeOutTime)
        {
            var notification = achievement.GetComponent<CanvasGroup>();

            int startAlpha = 0;
            int endAlpha = 1;

            for (int i = 0; i < 2; i++)
            {
                float rate = i == 0 ? 1f / fadeInTime : 1f / fadeOutTime;
                float progress = 0f;

                while (progress < 1f)
                {
                    notification.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
                    progress += rate * Time.deltaTime;
                    yield return null;
                }

                yield return new WaitForSeconds(downtime);
                startAlpha = 1;
                endAlpha = 0;
            }
            Destroy(achievement);
        }
    }
}
