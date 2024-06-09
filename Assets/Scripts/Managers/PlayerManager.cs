using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Larik.CardGame
{
    /// <summary>
    /// 玩家管理器
    /// </summary>
    public class PlayerManager
    {
        public List<ClientPlayer> playerList = new();

        private Func<ClientPlayer, DisplayCard, Promise> onPlayerPlayedCard;

        public void AddEventListener(Func<ClientPlayer, DisplayCard, Promise> onPlayerPlayedCard)
        {
            this.onPlayerPlayedCard = onPlayerPlayedCard;
        }

        /// <summary>
        /// 初始化玩家管理器
        /// </summary>
        public PlayerManager(ClientPlayer self, ClientPlayer opponet)
        {
            playerList.Add(self);
            playerList.Add(opponet);
    
            playerList.ForEach(player =>
            {
                player.AddEventListener(onPlayerPlayedCard);
            });
        }


        public List<string> MakeDeck(List<CardData> cardDataList)
        {
            List<string> list = new();
            playerList.ForEach(p =>
            {
                list.Add(p.MakeDeck(cardDataList));
            });
            return list;
        }

    }
}
