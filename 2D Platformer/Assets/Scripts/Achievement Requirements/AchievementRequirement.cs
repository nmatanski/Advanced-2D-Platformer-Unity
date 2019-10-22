using System;
using UnityEngine;

namespace Platformer.Achievements
{
    public class AchievementRequirement : MonoBehaviour, IAchievementRequirement
    {
        #region EventRequirements
        public static event Action<AchievementRequirement> OnTriggerEnteredAchievementRequirement;
        //public static event Action<AchievementRequirement> OnCollisionEnteredAchievementRequirement; ///TODO: Fix the collisions with charactercontroller
        public static event Action<AchievementRequirement> OnYKeyPressedAchievementRequirement;
        public static event Action<AchievementRequirement> OnJumpKeyPressedAchievementRequirement;
        public static event Action<AchievementRequirement> OnLeftRightKeysPressedAchievementRequirement;
        //public static event Action<AchievementRequirement> OnFullControlAchievementRequirement;
        #endregion

        #region RequirementsDTOs
        [SerializeField]
        private OnTriggerEnteredAchievementRequirementDTO pointOfInterestTriggerEnteredData;
        public OnTriggerEnteredAchievementRequirementDTO PointOfInterestTriggerEnteredData
        {
            get { return pointOfInterestTriggerEnteredData; }
            private set { pointOfInterestTriggerEnteredData = value; }
        }

        [SerializeField]
        private PointOfInterest achievementData;
        public PointOfInterest AchievementData
        {
            get { return achievementData; }
            set { achievementData = value; }
        }

        //public bool IsReady { get; set; } = false;

        //public int ReadyRequirements { get; set; } = 0;

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

        private void Update()
        {
            if (gameObject.CompareTag("CollisionAchievement"))
            {
                return;
            }
            switch (gameObject.tag)
            {
                case "YKeyReq":
                    OnYKeyPressed();
                    break;
                case "JumpButtonReq":
                    OnJumpKeyPressed();
                    break;
                case "HorizontalButtonReq":
                    OnLeftRightKeysPressed();
                    break;
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            FulfillRequirement(OnTriggerEnteredAchievementRequirement);
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    FulfillRequirement(OnCollisionEnteredAchievementRequirement);
        //}

        private void OnYKeyPressed()
        {
            if (Input.GetKey(KeyCode.Y))
            {
                FulfillRequirement(OnYKeyPressedAchievementRequirement);
            }
        }

        private void OnJumpKeyPressed()
        {
            if (Input.GetButtonDown("Jump"))
            {
                FulfillRequirement(OnJumpKeyPressedAchievementRequirement);
            }
        }

        private void OnLeftRightKeysPressed()
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                FulfillRequirement(OnLeftRightKeysPressedAchievementRequirement);
            }
        }

        //public void OnFullControl()
        //{
        //    if (++ReadyRequirements > 1)
        //    {
        //        FulfillRequirement(OnFullControlAchievementRequirement);
        //    }
        //}
    }
}
