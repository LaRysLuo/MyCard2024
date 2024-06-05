using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Larik.CardGame
{
    public class CardData
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id;
        /// <summary>
        /// 卡名
        /// </summary>
        public string cardName;
        /// <summary>
        /// 卡牌描述
        /// </summary>
        public string cardDesc;
        /// <summary>
        /// 卡牌类型
        /// </summary>
        public CardType cardType;

        /// <summary>
        /// 额外说明
        /// </summary>
        public string cardExtends;

        public CardData(int id, string cardName, CardType cardType, string cardDesc)
        {
            this.id = id;
            this.cardName = cardName;
            this.cardType = cardType;
            this.cardDesc = cardDesc;
        }
    }
}


