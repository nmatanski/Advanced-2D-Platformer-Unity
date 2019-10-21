using Platformer.Achievements.UI;
using Platformer.Managers;
using System.Collections.Generic;
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

        public string ChildAchievement { get; set; }

        private List<Achievement> requiredAchievements = new List<Achievement>();
        private string key;
        //private AchievementRequirement requirement;


        public Achievement(string name, string description, int points, int spriteIndex, GameObject achievementRef)
        {
            Name = name;
            Description = description;
            Points = points;
            SpriteIndex = spriteIndex;
            AchievementRef = achievementRef;
            key = AchievementManager.GetAchievementKey(name);

            Load();

            //requirement = GameObject.FindGameObjectWithTag("FullControlReq").GetComponent<AchievementRequirement>();

            Debug.Log("current points " + points);
        }

        public bool IsEarned(bool isUnlocked, string key)
        {
            this.key = key;
            if (!(IsUnlocked || requiredAchievements.Exists(req => !req.IsUnlocked)))
            {
                UnlockAchievementInAchievementMenu();
                Save(isUnlocked);

                if (ChildAchievement != null)
                {
                    //var achievementManager = new AchievementManager();
                    //string achKey = AchievementManager.GetAchievementKey(ChildAchievement);
                    //AchievementManagerUI.Instance.TryEarnAchievement(ChildAchievement, achievementManager.HasBeenAddedToPlayerPrefs(achKey), achKey);

                    //var requirement = new AchievementRequirement(new PointOfInterest(ChildAchievement.Replace(" ", string.Empty), ChildAchievement, ChildAchievement));
                    //requirement.OnFullControl();
                    //Debug.Log($"{requirement.ReadyRequirements}/2)"); ///TODO: Change the required ready count dynamically from serialized field
                }

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
            points += Points;

            PlayerPrefs.SetInt("AchievementPoints", points);

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

        public void AddRequiredAchievement(Achievement dependency)
        {
            requiredAchievements.Add(dependency);
        }


        private void UnlockAchievementInAchievementMenu()
        {
            AchievementRef.GetComponent<Image>().sprite = AchievementManagerUI.Instance.UnlockedAchievementSprite;
            AchievementRef.transform.SetSiblingIndex(0);
        }
    }
}
