using System;
using Platformer.Achievements;
using Platformer.Achievements.UI;
using UnityEngine;

namespace Platformer.Managers
{
    public class AchievementManager : MonoBehaviour, IManager
    {
        public ManagerStatus Status { get; private set; }

        [SerializeField]
        AchievementManagerUI achievementsUI;


        //private void Start()
        //{
        //    //Startup();
        //}

        private void OnDestroy()
        {
            AchievementRequirement.OnTriggerEnteredAchievementRequirement -= AchievementRequirement_OnTriggerEnteredAchievementRequirement;
            AchievementRequirement.OnYKeyPressedAchievementRequirement -= AchievementRequirement_OnYKeyPressedAchievementRequirement;
            //AchievementRequirement.OnCollisionEnteredAchievementRequirement -= AchievementRequirement_OnCollisionEnteredAchievementRequirement;
        }


        public void Startup()
        {
            Debug.Log("Achievement Manager is starting...");

            ///TODO: remove this at release
            PlayerPrefs.DeleteAll();
            AchievementRequirement.OnTriggerEnteredAchievementRequirement += AchievementRequirement_OnTriggerEnteredAchievementRequirement;
            AchievementRequirement.OnYKeyPressedAchievementRequirement += AchievementRequirement_OnYKeyPressedAchievementRequirement;
            //AchievementRequirement.OnCollisionEnteredAchievementRequirement += AchievementRequirement_OnCollisionEnteredAchievementRequirement;
            ///Attach more requirement events here

            ///

            Status = ManagerStatus.Started;
        }


        private void AchievementRequirement_OnTriggerEnteredAchievementRequirement(AchievementRequirement requirement)
        {
            string requirementKey = GetAchievementKey(requirement.PointOfInterestTriggerEnteredData.Name);
            achievementsUI.TryEarnAchievement(requirement.PointOfInterestTriggerEnteredData.Name, HasBeenAddedToPlayerPrefs(requirementKey), requirementKey); ///TODO: Implement it to send the achievementkey in the ui and there to call the add to playerprefs
        }

        //private void AchievementRequirement_OnCollisionEnteredAchievementRequirement(AchievementRequirement requirement)
        //{
        //    string achievementKey = GetAchievementKey(requirement.PointOfInterestCollisionEnteredData);
        //    HasBeenAddedToPlayerPrefs(achievementKey);

        //    Debug.Log($"Unlocked an achievement: {achievementKey}");
        //}

        private void AchievementRequirement_OnYKeyPressedAchievementRequirement(AchievementRequirement requirement)
        {
            string achievementKey = GetAchievementKey(requirement.AchievementData.Name);
            achievementsUI.TryEarnAchievement(requirement.AchievementData.Name, HasBeenAddedToPlayerPrefs(achievementKey), achievementKey); ///TODO: Implement it to send the achievementkey in the ui and there to call the add to playerprefs

            if (Input.GetKeyDown(KeyCode.Y))
            {
                achievementsUI.AchievementMenu.SetActive(!achievementsUI.AchievementMenu.activeSelf);
            }
        }


        public static string GetAchievementKey(string title)
        {
            return $"ID_Achievement_{title}";
        }


        private bool HasBeenAddedToPlayerPrefs(string key)
        {
            if (PlayerPrefs.GetInt(key) == 1)
                return true;

            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();

            return false;
        }
    }
}
