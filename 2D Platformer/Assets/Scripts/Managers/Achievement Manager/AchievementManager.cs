using System;
using Platformer.Achievements;
using UnityEngine;

namespace Platformer.Managers
{
    public class AchievementManager : MonoBehaviour, IManager
    {
        public ManagerStatus Status { get; private set; }


        private void Start()
        {
            //Startup();
        }

        private void OnDestroy()
        {
            AchievementRequirement.OnTriggerEnteredAchievementRequirement -= AchievementRequirement_OnTriggerEnteredAchievementRequirement;
            //AchievementRequirement.OnCollisionEnteredAchievementRequirement -= AchievementRequirement_OnCollisionEnteredAchievementRequirement;
        }


        public void Startup()
        {
            Debug.Log("Achievement Manager is starting...");

            PlayerPrefs.DeleteAll();
            AchievementRequirement.OnTriggerEnteredAchievementRequirement += AchievementRequirement_OnTriggerEnteredAchievementRequirement;
            //AchievementRequirement.OnCollisionEnteredAchievementRequirement += AchievementRequirement_OnCollisionEnteredAchievementRequirement;
            ///Attach more requirement events here

            ///

            Status = ManagerStatus.Started;
        }


        private void AchievementRequirement_OnTriggerEnteredAchievementRequirement(AchievementRequirement requirement)
        {
            string achievementKey = BuildAchievementKeyFromPointOfInterestDTO(requirement.PointOfInterestTriggerEnteredData);
            AddAchievementToPlayerPrefs(achievementKey);

            Debug.Log($"Unlocked an achievement: {achievementKey}\n{requirement.PointOfInterestTriggerEnteredData.Description}");
        }

        //private void AchievementRequirement_OnCollisionEnteredAchievementRequirement(AchievementRequirement requirement)
        //{
        //    string achievementKey = BuildAchievementKeyFromPointOfInterestDTO(requirement.PointOfInterestCollisionEnteredData);
        //    AddAchievementToPlayerPrefs(achievementKey);

        //    Debug.Log($"Unlocked an achievement: {achievementKey}");
        //}


        private string BuildAchievementKeyFromPointOfInterestDTO(PointOfInterest poi)
        {
            return $"ID_Achievement_{poi.Title}";
        }

        private void AddAchievementToPlayerPrefs(string key)
        {
            if (PlayerPrefs.GetInt(key) == 1)
                return;

            PlayerPrefs.SetInt(key, 1);
        }
    }
}
