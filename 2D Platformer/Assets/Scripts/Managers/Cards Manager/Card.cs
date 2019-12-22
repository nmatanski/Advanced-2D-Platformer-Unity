namespace Platformer.Managers
{
    public class Card
    {
        public static readonly string[] Suits = { "hearts", "diamonds", "spades", "clubs" };

        public static readonly string[] Values = { "Ace", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "Jack", "Queen", "King", "Joker" };


        public string StringValue { get; private set; }

        public string Suit { get; private set; }

        public int Value { get; private set; }


        public Card(string name, string suit, int value)
        {
            StringValue = name;
            Suit = suit;
            Value = value;
        }
    }
}
