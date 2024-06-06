using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace Larik.CardGame
{
    /// <summary>
    /// 游戏的总控制器
    /// 1. 初始化
    /// 1.1 玩家初始化
    /// 1.2 玩家UI初始化
    /// 1.3 效果管理器初始化
    /// 2. 生成牌组
    /// 3. 开场为双方玩家抽取手牌
    /// 4. 游戏开始
    /// </summary>
    public class GameFacede
    {
        public PlayerManager playerManager;
        public EffectManager effectManager;

        public DataManager dataManager;

        public TurnManager turnManager;

        public InputManager inputManager;

        public void InitGame()
        {
            Debug.Log("游戏初始化");
            InitPlayers();
            InitEffectManager();
            InitDataManager();
            InitTurnManager();
            InitInputManager();
        }

        #region 初始化
        /// <summary>
        /// 初始化玩家
        /// </summary>
        private void InitPlayers()
        {
            ClientPlayer selfPlayer = new(0);
            ClientPlayer opponentPlayer = new(1);
            playerManager = new(selfPlayer, opponentPlayer);
            playerManager.AddEventListener(OnPlayerPlayedCard);
        }

        private void InitEffectManager()
        {
            effectManager = new();
        }

        private void InitDataManager()
        {
            dataManager = new();
        }

        private void InitTurnManager()
        {
            turnManager = new();
            turnManager.AddEventListener(OnTurnStart);
        }

        private void InitInputManager()
        {
            inputManager = new();
            inputManager.AddEventListener(OnPlayerInputPlayCard);
        }
        #endregion

        /// <summary>
        /// 连接inputManager和Gamefacade
        /// </summary>
        /// <param name="source"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        public Promise OnPlayerInputPlayCard(ClientPlayer source, DisplayCard card)
        {
            return source.PlayCard(card);
        }

        /// <summary>
        /// 事件：当玩家发动了卡牌
        /// 连接playerManager和EffectManager
        /// </summary>
        /// <param name="source">触发玩家</param>
        /// <param name="card">触发卡牌</param>
        /// <returns></returns>
        private Promise OnPlayerPlayedCard(ClientPlayer source, DisplayCard card)
        {
            effectManager.TriggerEffect(source, card);
            return new();
        }


        /// <summary>
        /// 随机生成卡组
        /// </summary>
        public List<string> MakeDeck()
        {
            return playerManager.MakeDeck(dataManager.cardDataList);
        }


        /// <summary>
        /// 根据实际的卡牌数据生成牌组
        /// 1 - 两位玩家的数据用卡牌的id用逗号分隔，拼接在一起
        /// 2 - 如果是网络化的情况，通过网络将卡牌数据发送到此方法处理
        /// </summary>
        /// <param name="playersDeckData">两个玩家的卡牌数据</param>
        public void BuildDeck(List<string> playersDeckData)
        {
            int count = 0;
            playerManager.playerList.ForEach(player =>
            {
                string deckDataStr = playersDeckData[count];
                List<CardData> deckData = deckDataStr.Split(',').ToList().Select(id => dataManager.cardDataList.ToList().Find(c => c.id == int.Parse(id))).ToList();
                player.BuildDeck(deckData, inputManager.OnPlayerAction);
            });
        }

        /// <summary>
        /// 开场为双方玩家抽取手牌
        /// 1. 抽取5张手牌
        /// </summary>
        public void DrawFirstHand()
        {
            playerManager.playerList.ForEach(player =>
            {
                player.DrawFirstHand();
            });
        }

        /// <summary>
        /// 准备就绪，游戏开始
        /// </summary>
        public void StartGame()
        {
            turnManager.StartGame();
        }

        public ClientPlayer TurnPlayer(int current)
        {
            return playerManager.playerList[current];
        }

        /// <summary>
        /// 事件：回合开始了
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private Promise OnTurnStart(int current, int turns)
        {
            Promise turnPromise = new();
            Promise.SequentialCall(new(){
                //1. 抽1张牌
                ()=>TurnPlayer(current).DrawCard(1),
                //2. 检查在此时要发动的牌
                ()=>TurnPlayer(current).HandleTurnStartEffect(),
                //3. 等待玩家进行回合
                ()=> inputManager.Active(TurnPlayer(current))

            }, 0.1f).OnComplete(_ =>
            {
                //回合结束
                turnPromise.Resolve();
            });
            return turnPromise;
        }

    }

}
