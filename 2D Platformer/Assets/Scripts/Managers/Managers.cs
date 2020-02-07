using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Managers
{
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(InventoryManager))]
    [RequireComponent(typeof(CardsManager))]
    [RequireComponent(typeof(AchievementManager))]
    public class Managers : MonoBehaviour
    {
        public static PlayerManager Player { get; private set; }
        public static InventoryManager Inventory { get; private set; }
        public static CardsManager Deck { get; private set; }
        public static AchievementManager Achievements { get; private set; }

        public bool AreReady { get; private set; } = false;

        private List<IManager> _startSequence;

        void Awake()
        {
            Player = GetComponent<PlayerManager>();
            Inventory = GetComponent<InventoryManager>();
            Deck = GetComponent<CardsManager>();
            Achievements = GetComponent<AchievementManager>();

            _startSequence = new List<IManager>();
            _startSequence.Add(Player);
            _startSequence.Add(Inventory);
            _startSequence.Add(Deck);
            _startSequence.Add(Achievements);

            StartCoroutine(StartupManagers());
        }

        private IEnumerator StartupManagers()
        {
            foreach (IManager manager in _startSequence)
            {
                try
                {
                    manager.Startup();
                }
                catch (NullReferenceException)
                {
                    Debug.LogWarning($"NullReferenceException: One of the managers is missing!");
                }
            }

            yield return null;

            int numModules = _startSequence.Count;
            int numReady = 0;

            while (numReady < numModules)
            {
                int lastReady = numReady;
                numReady = 0;

                foreach (IManager manager in _startSequence)
                {
                    if (manager.Status == ManagerStatus.Started)
                    {
                        numReady++;
                    }
                }

                if (numReady > lastReady)
                    Debug.Log("Progress: " + numReady + "/" + numModules);

                yield return null;
            }

            Debug.Log("All managers started up");
            AreReady = true;
        }
    }
}
