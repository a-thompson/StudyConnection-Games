using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study
{
    public class CardSet
    {
        public CardSet()
        {
            //default demo card set
            set = "Demo 1";
            cards = new List<FlashCard>();
            createCards();
        }
        public CardSet(String theSet, List<FlashCard> theCards)
        {
            set = theSet;
            cards = theCards;
        }
        public CardSet(CardSet Object)
        {
            set = Object.set;
            cards = Object.cards;
        }
        private void createCards()
        {
            FlashCard card1, card2, card3, card4;
            List<String> list1, list2, list3, list4;
            String topic1, topic2, topic3, topic4;
            //create cards
            list1 = new List<string>();
            topic1 = "4 Basic Activities in Interaction Design";
            list1.Add("establishing requirements");
            list1.Add("designing alternatives");
            list1.Add("prototyping");
            list1.Add("evaluating");
            card1 = new FlashCard(topic1, list1);

            list2 = new List<string>();
            topic2 = "User Experience";
            list2.Add("how a product behaves and is used in the real world");
            card2 = new FlashCard(topic2, list2);

            list3 = new List<string>();
            topic3 = "Low Fidelity Prototype";
            list3.Add("uses a medium to interpret the design");
            list3.Add("ie: paper model");
            card3 = new FlashCard(topic3, list3);

            list4 = new List<string>();
            topic4 = "Internal Consistancy";
            list4.Add("design operations behave the same within an application");
            card4 = new FlashCard(topic4, list4);

            //add demo cards to set
            cards.Add(card1);
            cards.Add(card2);
            cards.Add(card3);
            cards.Add(card4);

        }
        public void Shuffle()
        {
            Random random = new Random();
            int num = cards.Count;

            for (int i = num - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                FlashCard value = cards[rnd];
                cards[rnd] = cards[i];
                cards[i] = value;
            }
        }
        public void Rotate()
        {
            FlashCard first = cards.First();
            cards.Remove(first);
            cards.Add(first);
        }

        private String set;
        private List<FlashCard> cards;

        public void setSet(String theSet)
        {
            set = theSet;
        }
        public void setCards(List<FlashCard> theCards)
        {
            cards = theCards;
        }
        public String getSet()
        {
            return set;
        }
        public List<FlashCard> getCards()
        {
            return cards;
        }

        public String toString()
        {
            return (set);
        }
        public bool equals(CardSet Object)
        {
            return ((Object.set.Equals(set) && (Object.cards.Equals(cards))));
        }
    }
}
