using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Larik.CardGame
{
    public class Deck : CardGrid
    {
        /// <summary>
        /// 生成牌组数据
        /// </summary>
        public string Make(List<CardData> cardDataList)
        {
            string str = "";
            //随机挑选几张卡牌，组成40张的牌组
            for (int i = 0; i < 40; i++)
            {
                if (str != "") str += ",";
                int rand = UnityEngine.Random.Range(0, cardDataList.Count);
                int cardId = cardDataList[rand].id;
                str += cardId;
            }
            return str;

        }
        /// <summary>
        /// 根据卡牌数据的列表生成卡组
        /// </summary>
        /// <param name="list"></param>
        public void Build(List<CardData> list, Action<string, DisplayCard, Action<bool>> onCardPlayed)
        {
            //将卡牌数据转换为卡牌对象
            List<DisplayCard> cardList = list.Select(data => GameObject.Instantiate<DisplayCard>(AssetManager.DisplayCardPrefab)).ToList();
            //初始化卡牌，按照数组顺序设置卡牌ID
            int index = 0;
            cardList.ForEach((card) =>
            {
                Debug.Log("11" + list[index]);
                CardInfo cardInfo = new(index, list[index]);
                card.Init(cardInfo, onCardPlayed);
                index++;
            });
            //将卡牌对象放入牌组
            cardList.ForEach(card => Add(card));
        }

        public DisplayCard Draw()
        {
            //从牌组最上方拿到1张卡
            return (DisplayCard)CardList.Take(1);
        }
    }

}

