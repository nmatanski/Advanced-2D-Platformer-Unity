using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Deck = new Deck(); ///TODO: Or load from savefile!!!!!

            Status = ManagerStatus.Started;
            ///long-runing startups tasks here
        }

        public void ResetDeck()
        {
            Deck = Deck.Reset();
        }

        public Card DrawCard()
        {
            return Deck.Draw();
        }
    }
}
