using System;
using UnityEngine;

namespace Larik.CardGame
{
    /// <summary>
    /// 玩家操作的时期
    /// </summary>
    public class InputManager
    {
        private Promise inputPromise;

        private bool inputEnable = false;

        private ClientPlayer turnPlayer;

        private Func<ClientPlayer, DisplayCard, Promise> onPlayerPlayCard;

        public void AddEventListener(Func<ClientPlayer, DisplayCard, Promise> onPlayerPlayCard)
        {
            this.onPlayerPlayCard = onPlayerPlayCard;
        }

        /// <summary>
        /// 激活输入管理器
        /// </summary>
        public Promise Active(ClientPlayer turnPlayer)
        {
            inputPromise = new();
            inputEnable = true;
            this.turnPlayer = turnPlayer;
            return inputPromise;
        }

        public void OnPlayerAction(string type, ClientPlayer source, DisplayCard card, Action<bool> actions)
        {
            Debug.Log(123);
            // if (!source.IsLocalPlayer) actions?.Invoke(false);
            // if (turnPlayer != source) actions?.Invoke(false);
            if (type == "onBeginDrag") HandleStartDrag(actions);
            if (type == "onDrag") HandleOnDrag(actions);
            if (type == "onEndDrag") HandleEndDrag(actions);
            // if (type == "onPlayed") return HandlePlayedCard(source, card);
        }

        private bool isDraging = false;

        private bool Dragable()
        {
            // if (!inputEnable) return false;
            if (isDraging) return false;
            return true;
        }

        private bool Viewable()
        {

            return true;
        }

        /// <summary>
        /// 处理拖动
        /// </summary>
        private void HandleStartDrag(Action<bool> actions)
        {
            if (!Dragable())
            {
                Debug.LogError(111);
                actions?.Invoke(false);
                return;
            }
            isDraging = true;
            actions?.Invoke(true);
        }

        private void HandleOnDrag(Action<bool> callback)
        {
            if (!isDraging)
            {
                callback?.Invoke(false);
                return;
            }
            callback?.Invoke(true);
        }

        /// <summary>
        /// 处理结束拖动
        /// </summary>
        /// <returns></returns>
        private void HandleEndDrag(Action<bool> actions)
        {
            if (!Dragable())
            {
                actions?.Invoke(false);
                return;
            }
            isDraging = false;
            actions?.Invoke(true);
        }

        /// <summary>
        /// 处理玩家打出卡牌
        /// </summary>
        /// <param name="source">触发玩家</param>
        /// <param name="card">触发卡牌</param>
        /// <returns></returns>
        private Promise HandlePlayedCard(ClientPlayer source, DisplayCard card)
        {
            Promise pms = onPlayerPlayCard?.Invoke(source, card);
            return pms;
        }

        /// <summary>
        /// 处理放大观看
        /// </summary>
        private void HandleView() { }

        /// <summary>
        /// 打出卡牌
        /// </summary>
        public void OnPlayedCard() { }

        /// <summary>
        /// 当回合结束按钮被按下时或因为其他原因回合结束
        /// </summary>
        public void OnTurnEnd()
        {
            inputPromise.Resolve();
        }
    }
}