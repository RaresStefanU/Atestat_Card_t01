using System;
using System.Collections;
//using System.Linq;
using System.Text;

namespace CardLib
{
    public class CardCollection : CollectionBase, ICloneable
    {
        public object Clone()
        {
            CardCollection newCards = new CardCollection();
            foreach (Card sourceCard in List)
            {
                newCards.Add((Card)sourceCard.Clone());
            }
            return newCards;
        }

        public void Add(Card newCard) => List.Add(newCard);

        public void Remove(Card oldCard) => List.Remove(oldCard);

        public Card this[int cardIndex]
        {
            get { return (Card)List[cardIndex]; }
            set { List[cardIndex] = value; }
        }

        //copiaza instantele 'cardului'...

        public void CopyTo(CardCollection targetCards)
        {
            for (int index = 0; index < this.Count; index++)
            {
                targetCards[index] = this[index];
            }
        }
        //insereaza un sumar

        public bool Contains(Card card) => InnerList.Contains(card);
    }
}