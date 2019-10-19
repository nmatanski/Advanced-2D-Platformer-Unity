using Platformer.Achievements.UI;
using Platformer.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Achievements
{
    public class Achievement
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsUnlocked { get; private set; }
        public int Points { get; private set; }
        public int SpriteIndex { get; private set; }
        public GameObject AchievementRef { get; private set; }

        private string key;


        public Achievement(string name, string description, int points, int spriteIndex, GameObject achievementRef)
        {
            Name = name;
            Description = description;
            Points = points;
            SpriteIndex = spriteIndex;
            AchievementRef = achievementRef;
            key = AchievementManager.GetAchievementKey(name);

            Load();

            Debug.Log("current points " + points);
        }

        public bool IsEarned(bool isUnlocked, string key)
        {
            this.key = key;
            if (!IsUnlocked)
            {
                UnlockAchievementInAchievementMenu();
                Save(isUnlocked);
                return false;
            }

            return true;
        }

        public void Save(bool value)
        {
            IsUnlocked = value;

            if (IsUnlocked)
            {
                return;
            }

            int points = PlayerPrefs.GetInt("AchievementPoints");
            Debug.Log("current points " + points);
            Points += points;

            PlayerPrefs.SetInt("AchievementPoints", Points);

            PlayerPrefs.Save();

            ///TODO: Test
            AchievementManagerUI.Instance.PointsText.text = $"Achievement points: {PlayerPrefs.GetInt("AchievementPoints")}";
        }

        public void Load()
        {
            IsUnlocked = PlayerPrefs.GetInt(key) == 1;
            AchievementManagerUI.Instance.PointsText.text = $"Achievement points: {PlayerPrefs.GetInt("AchievementPoints")}";

            if (IsUnlocked)
            {
                UnlockAchievementInAchievementMenu();
            }
        }


        private void UnlockAchievementInAchievementMenu()
        {
            AchievementRef.GetComponent<Image>().sprite = AchievementManagerUI.Instance.UnlockedAchievementSprite;
        }
    }
}
