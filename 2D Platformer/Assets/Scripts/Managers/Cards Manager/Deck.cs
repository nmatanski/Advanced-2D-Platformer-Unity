using System;
using System.Collections.Generic;

namespace Platformer.Managers
{
    public class Deck
    {
        public List<Card> Cards { get; private set; }


        public Deck()
        {
            Reset();
        }


        public Deck Reset()
        {
            Cards = new List<Card>();

            var suits = Card.Suits;
            var stringValues = Card.Values;

            foreach (var suit in suits)
                for (int i = 0; i < stringValues.Length; i++)
                    Cards.Add(new Card(stringValues[i], suit, i + 1));

            Shuffle();

            return this;
        }

        public Card Draw()
        {
            var topCard = Cards[0];
            Cards.RemoveAt(0);

            if (topCard.StringValue == "Joker")
            {
                Reset();
            }

            return topCard;
        }

        private void Shuffle()
        {
            var random = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber) + (int)DateTime.Now.Ticks / 2);
            for (int i = Cards.Count - 1; i > 0; i--)
            {
                int randomIndex = random.Next(i);
                // Swap
                var tempCard = Cards[randomIndex];
                Cards[randomIndex] = Cards[i];
                Cards[i] = tempCard;
            }
        }
    }
}
