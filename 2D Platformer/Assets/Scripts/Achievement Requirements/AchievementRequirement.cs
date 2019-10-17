using System;
using UnityEngine;

namespace Platformer.Achievements
{
    public class AchievementRequirement : MonoBehaviour, IAchievementRequirement
    {
        #region EventRequirements
        public static event Action<AchievementRequirement> OnTriggerEnteredAchievementRequirement;
        //public static event Action<AchievementRequirement> OnCollisionEnteredAchievementRequirement; ///TODO: Fix the collisions with charactercontroller
        #endregion

        #region RequirementsDTOs
        [SerializeField]
        private OnTriggerEnteredAchievementRequirementDTO pointOfInterestTriggerEnteredData;
        public OnTriggerEnteredAchievementRequirementDTO PointOfInterestTriggerEnteredData
        {
            get { return pointOfInterestTriggerEnteredData; }
            private set { pointOfInterestTriggerEnteredData = value; }
        }

        ///TODO: Add this when I find a raycasting collision fix
        //[SerializeField]
        //private OnCollisionEnteredAchievementRequirementDTO pointOfInterestCollisionEnteredData;

        //public OnCollisionEnteredAchievementRequirementDTO PointOfInterestCollisionEnteredData
        //{
        //    get { return pointOfInterestCollisionEnteredData; }
        //    set { pointOfInterestCollisionEnteredData = value; }
        //}
        #endregion


        public void FulfillRequirement(Action<AchievementRequirement> onAchievementRequirementFulfilled)
        {
            onAchievementRequirementFulfilled?.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            FulfillRequirement(OnTriggerEnteredAchievementRequirement);
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    FulfillRequirement(OnCollisionEnteredAchievementRequirement);
        //}
    }
}
