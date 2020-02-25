using System.Collections;
using Platformer.Achievements;
using Platformer.Achievements.UI;
using UnityEngine;

namespace Platformer.Managers
{
    public class AchievementManager : MonoBehaviour, IManager
    {
        public ManagerStatus Status { get; private set; }

        [SerializeField]
#pragma warning disable CS0649 // Field 'AchievementManager.achievementsUI' is never assigned to, and will always have its default value null
        AchievementManagerUI achievementsUI;
#pragma warning restore CS0649 // Field 'AchievementManager.achievementsUI' is never assigned to, and will always have its default value null


        private void OnDestroy()
        {
            AchievementRequirement.OnTriggerEnteredAchievementRequirement -= AchievementRequirement_OnTriggerEnteredAchievementRequirement;
            AchievementRequirement.OnYKeyPressedAchievementRequirement -= AchievementRequirement_OnYKeyPressedAchievementRequirement;
            AchievementRequirement.OnJumpKeyPressedAchievementRequirement -= AchievementRequirement_OnJumpKeyPressedAchievementRequirement;
            AchievementRequirement.OnLeftRightKeysPressedAchievementRequirement -= AchievementRequirement_OnLeftRightKeysPressedAchievementRequirement;
            //AchievementRequirement.OnCollisionEnteredAchievementRequirement -= AchievementRequirement_OnCollisionEnteredAchievementRequirement;
        }


        public void Startup()
        {
            Debug.Log("Achievement Manager is starting...");

            Status = ManagerStatus.Initializing;

            StartCoroutine(LoadAndCache());
        }


        private void AchievementRequirement_OnTriggerEnteredAchievementRequirement(AchievementRequirement requirement)
        {
            TryEarnAchievement(requirement.PointOfInterestTriggerEnteredData);
        }

        //private void AchievementRequirement_OnCollisionEnteredAchievementRequirement(AchievementRequirement requirement)
        //{
        //    string achievementKey = GetAchievementKey(requirement.PointOfInterestCollisionEnteredData);
        //    HasBeenAddedToPlayerPrefs(achievementKey);

        //    Debug.Log($"Unlocked an achievement: {achievementKey}");
        //}

        private void AchievementRequirement_OnYKeyPressedAchievementRequirement(AchievementRequirement requirement)
        {
            TryEarnAchievement(requirement.AchievementData);

            if (Input.GetKeyDown(KeyCode.Y))
            {
                achievementsUI.AchievementMenu.SetActive(!achievementsUI.AchievementMenu.activeSelf);
            }
        }

        private void AchievementRequirement_OnJumpKeyPressedAchievementRequirement(AchievementRequirement requirement)
        {
            TryEarnAchievement(requirement.AchievementData);
        }

        private void AchievementRequirement_OnLeftRightKeysPressedAchievementRequirement(AchievementRequirement requirement)
        {
            TryEarnAchievement(requirement.AchievementData);
        }


        private void TryEarnAchievement(PointOfInterest dto)
        {
            string achievementKey = GetAchievementKey(dto.Name);
            achievementsUI.TryEarnAchievement(dto.Name, HasBeenAddedToPlayerPrefs(achievementKey), achievementKey);
        }

        public static string GetAchievementKey(string title)
        {
            return $"ID_Achievement_{title}";
        }


        public bool HasBeenAddedToPlayerPrefs(string key)
        {
            if (PlayerPrefs.GetInt(key) == 1)
                return true;

            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();

            return false;
        }

        public IEnumerator LoadAndCache()
        {
            yield return StartCoroutine(Load());

            Status = ManagerStatus.Started;
            Debug.Log(Status);
        }

        public IEnumerator Load()
        {
            ///TODO: remove this at release
            PlayerPrefs.DeleteAll();
            AchievementRequirement.OnTriggerEnteredAchievementRequirement += AchievementRequirement_OnTriggerEnteredAchievementRequirement;
            AchievementRequirement.OnYKeyPressedAchievementRequirement += AchievementRequirement_OnYKeyPressedAchievementRequirement;
            AchievementRequirement.OnJumpKeyPressedAchievementRequirement += AchievementRequirement_OnJumpKeyPressedAchievementRequirement;
            AchievementRequirement.OnLeftRightKeysPressedAchievementRequirement += AchievementRequirement_OnLeftRightKeysPressedAchievementRequirement;
            //AchievementRequirement.OnCollisionEnteredAchievementRequirement += AchievementRequirement_OnCollisionEnteredAchievementRequirement;
            ///Attach more requirement events here

            ///


            yield return null;
        }
    }
}
