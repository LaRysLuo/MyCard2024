namespace Larik.CardGame
{
    [System.Serializable]
    public class CardInfo
    {
        public int id;
        public int cardDataId;
        public CardType cardType;
        public string name;
        public string desc;
        public string extend;

        public CardInfo(int id, CardData cardData)
        {
            this.id = id;
            this.cardDataId = cardData.id;
            this.cardType = cardData.cardType;
            this.name = cardData.cardName;
            this.desc = cardData.cardDesc;
            this.extend = cardData.cardExtends;
        }


    }
}