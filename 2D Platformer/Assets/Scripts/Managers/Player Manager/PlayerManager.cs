using UnityEngine;

namespace Platformer.Managers
{
    public class PlayerManager : MonoBehaviour, IManager
    {
        public ManagerStatus Status { get; private set; }

        public int Health { get; private set; }

        public int MaxHealth { get; private set; }

        [SerializeField]
        private SelectedObject lastSelectedObject;
        public SelectedObject LastSelectedObject
        {
            get { return lastSelectedObject; }
            set { lastSelectedObject = value; }
        }


        public void Startup()
        {
            Debug.Log("Player Manager is starting...");

            Health = 100; ///TODO: load this from savefile
            MaxHealth = 100; ///TODO: load this from savefile

            Status = ManagerStatus.Started;
            ///long-runing startups tasks here
        }

        private void Update()
        {
            if (!LastSelectedObject.HasSelectedObject)
            {
                LastSelectedObject.SelectedGameObject = null;
            }
        }


        /// <param name="value">Could be both positive and negative value</param>
        public void AddHealth(int value)
        {
            Health = Mathf.Clamp(Health + value, 0, MaxHealth);

            Debug.Log($"Health: {Health}/{MaxHealth}");
        }
    }
}
