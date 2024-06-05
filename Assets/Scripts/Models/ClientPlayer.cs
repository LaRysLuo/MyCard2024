using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Larik.CardGame
{
    public class ClientPlayer
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id;
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string name;

        /// <summary>
        /// 血量
        /// </summary>
        public int lifePoint = 30;

        public PlayerView playerView;

        private Func<ClientPlayer, DisplayCard, Promise> onPlayerPlayedCard;

        /// <summary>
        /// 判断该玩家是否为本地玩家
        /// </summary>
        /// <value></value>
        public bool IsLocalPlayer
        {
            get
            {
                return playerView.gameObject.name == "self";
            }
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="onPlayerPlayedCard"></param>
        public void AddEventListener(Func<ClientPlayer, DisplayCard, Promise> onPlayerPlayedCard)
        {
            this.onPlayerPlayedCard = onPlayerPlayedCard;
        }

        public ClientPlayer(int index)
        {
            string symbol = index == 0 ? "self" : "opponent";
            playerView = GameObject.Instantiate(AssetManager.PlayerViewPrefab, AssetManager.BattleGrid).GetComponent<PlayerView>();
            playerView.gameObject.name = symbol;
        }

        /// <summary>
        /// 生成牌组
        /// </summary>
        public string MakeDeck(List<CardData> cardDataList)
        {
            return playerView.Deck.Make(cardDataList);
        }

        /// <summary>
        /// 根据卡组数据生成牌组
        /// </summary>
        /// <param name="cardDataList"></param>
        public void BuildDeck(List<CardData> cardDataList, Action<string, ClientPlayer, DisplayCard, Action<bool>> onPlayerAction)
        {
            playerView.Deck.Build(cardDataList, (type, card, actions) => onPlayerAction?.Invoke(type, this, card, actions));
        }

        /// <summary>
        /// 处理回合开始时的效果
        /// </summary>
        public Promise HandleTurnStartEffect()
        {
            return new();
        }

        /// <summary>
        /// 等待玩家操作
        /// </summary>
        /// <returns></returns>
        public Promise WaitingForAction()
        {
            Promise pms = new();

            return pms;
        }

        /// <summary>
        /// 抽取第一次手牌
        /// </summary>
        /// <returns></returns>
        public Promise DrawFirstHand()
        {
            return DrawCard(5);
        }

        /// <summary>
        /// 抽牌
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public Promise DrawCard(int count)
        {
            Promise pms = new();
            List<Func<Promise>> actions = new();
            for (int i = 0; i < count; i++)
            {
                actions.Add(DrawCard);
            }
            Promise.SequentialCall(actions, 0.1f).OnComplete(_ =>
            {
                pms.Resolve();
            });
            return pms;
        }

        /// <summary>
        /// 从牌组抽1张牌
        /// </summary>
        /// <returns></returns>
        private Promise DrawCard()
        {
            Promise pms = new();
            DisplayCard card = playerView.Deck.Draw();
            LeanTween.move(card.gameObject, playerView.Hand.transform, 0.3f).setOnComplete(_ =>
            {
                playerView.Hand.Add(card);
                pms.Resolve();
            });
            return pms;
        }

        /// <summary>
        /// 丢弃多张手牌
        /// </summary>
        /// <param name="list">这是要丢弃的手牌</param>
        /// <returns></returns>
        public Promise DiscardCard(List<DisplayCard> list)
        {
            Promise pms = new();
            Promise.SequentialCall(list.ConvertAll(card => new Func<Promise>(() => DiscardCard(card)))).OnComplete(_ =>
            {
                pms.Resolve();
            });
            return pms;
        }

        /// <summary>
        /// 弃牌
        /// </summary>
        /// <param name="card">要弃的手牌</param>
        public Promise DiscardCard(DisplayCard card)
        {
            Promise pms = new();
            bool isHandCard = playerView.Hand.Contains(card);
            if (!isHandCard) return pms.Reject("不是这位玩家的手牌");
            LeanTween.move(card.gameObject, playerView.Discard.transform, 0.3f).setOnComplete(() =>
            {
                playerView.Discard.Add(card);
                pms.Resolve();
            });
            return pms;
        }

        /// <summary>
        /// 出牌
        /// </summary>
        public Promise PlayCard(DisplayCard card)
        {
            Promise pms = new();
            //1. 判断是否在该玩家的手牌
            if (!playerView.Hand.Contains(card)) return pms.Reject("不是这位玩家的手牌");
            //2. 将卡牌放置到场地上
            //3. 执行卡牌效果
            //4. 将卡牌移动到弃牌区
            Promise.SequentialCall(new(){
                () => PutCard(card),
                () => onPlayerPlayedCard(this,card),
                ()=> DiscardCard(card),
            }, 0.1f).OnComplete(_ =>
            {
                pms.Resolve();
            });
            return pms;
        }

        /// <summary>
        /// 将卡牌放置到场地上
        /// </summary>
        /// <param name="card">要放置的卡牌</param>
        /// <returns></returns>
        public Promise PutCard(DisplayCard card)
        {
            Promise pms = new();
            //2. 将卡牌放置到场地上
            LeanTween.move(card.gameObject, playerView.Field.transform, 0.3f).setOnComplete(() =>
            {
                playerView.Field.Add(card);
                pms.Resolve();
            });
            return pms;
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="val">伤害值</param>
        public void OnDamage(int val)
        {
            HandleDamageEffect(ref val);
            lifePoint -= val;
        }

        /// <summary>
        /// 处理影响伤害值的效果
        /// </summary>
        /// <param name="val"></param>
        private void HandleDamageEffect(ref int val)
        {

        }

    }

}
