using System.Collections;
using UnityEngine;

namespace Platformer.Managers
{
    public class CardsManager : MonoBehaviour, IManager
    {
        public ManagerStatus Status { get; private set; }

        public Deck Deck { get; private set; }


        public void Startup()
        {
            Debug.Log("Cards Manager is starting...");

            Status = ManagerStatus.Initializing;
            ///long-runing startups tasks here
            StartCoroutine(LoadAndCache());
        }

        public void ResetDeck()
        {
            Deck = Deck.Reset();
        }

        public Card DrawCard()
        {
            return Deck.Draw();
        }

        public IEnumerator LoadAndCache()
        {
            yield return StartCoroutine(Load());

            Status = ManagerStatus.Started;
            Debug.Log(Status);
        }

        public IEnumerator Load()
        {
            Deck = new Deck(); ///TODO: Or load from savefile!!!!!

            yield return null;
        }
    }
}
