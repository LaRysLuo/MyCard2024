using System;
using UnityEngine;
using UnityEngine.UI;

namespace Larik.CardGame
{
    /// <summary>
    /// 玩家的输入控制器
    /// 本地玩家的回合开始时，输入控制器就会启动，可以进行以下操作
    /// 1. 打出卡牌：拖动卡牌到场地上，就可以打出该卡牌并发动效果
    /// 2. 查看卡牌详情：在左上角显示一个展示卡牌数据的面板。这个面板在有其他卡牌效果触发时，也会切换显示
    /// 3. 结束回合：点击回合结束按钮，或者一些效果的特殊触发强制结束。
    /// 非自己回合时，可进行以下查看：
    /// 1. 点击查看场地上的卡牌。
    /// 2. 点击查看自己手上。
    /// 3. 点击查看双方墓地里的卡牌，用一个面板显示出来
    /// 4. 点击空场地可以关闭当前正在显示的卡牌数据面板
    /// 在对手回合出现可连锁的卡牌时
    /// </summary>
    public class InputManager
    {
        private Button TurnEndBtn => GameFacede.Get().uiManager.turnEndBtn;

        private Promise inputPromise;
        private ClientPlayer turnPlayer;

        private Func<ClientPlayer, DisplayCard, Promise> onPlayerPlayCard;

        /// <summary>
        /// 初始化添加事件监听，一次游戏触发1次
        /// </summary>
        /// <param name="onPlayerPlayCard"></param>
        public void AddEventListener(Func<ClientPlayer, DisplayCard, Promise> onPlayerPlayCard)
        {
            this.onPlayerPlayCard = onPlayerPlayCard;
            TurnEndBtn.onClick.AddListener(HandleTurnEnd);
        }

        /// <summary>
        /// 激活输入管理器
        /// </summary>
        public Promise Active(ClientPlayer turnPlayer)
        {
            this.turnPlayer = turnPlayer;
            inputPromise = new();
            //如果不是本地玩家，返回等待其他玩家操作
            if (!turnPlayer.IsLocalPlayer) return inputPromise;
            Debug.LogWarning("现在是你的回合");
            inputEnable = true;
            HandleShowTurnEndBtn();
            return inputPromise;
        }

        public void OnPlayerAction(string type, ClientPlayer source, DisplayCard card, Action<bool> actions)
        {
            // if (!source.IsLocalPlayer) actions?.Invoke(false);
            // if (turnPlayer != source) actions?.Invoke(false);
            if (type == "onBeginDrag") HandleStartDrag(card, actions);
            if (type == "onDrag") HandleOnDrag(actions);
            if (type == "onEndDrag") HandleEndDrag(card, actions);
            if (type == "onView") HandleView(card, actions);
            if (type == "onPlayed") HandlePlayedCard(source, card);
        }

        private bool inputEnable = false;
        private bool isDraging = false;

        /// <summary>
        /// 是否可拖动
        /// 1. 只有自己的手牌能拖动
        /// 2. 只有自己的回合才能拖动卡牌
        /// 3. 效果处理中不能拖动卡牌
        /// </summary>
        /// <returns></returns>
        private bool Dragable(DisplayCard triggerCard)
        {
            if (!(triggerCard.IsInHand && triggerCard.IsLocalCard)) return false; //只有自己的手牌能拖动
            if (!inputEnable) return false; //只有自己的回合才能拖动
            if (isDraging) return false; //如果已经在拖动其他卡牌了不能再拖动
            return true;
        }

        /// <summary>
        /// 是否可查看卡牌
        /// </summary>
        /// <param name="triggerCard"></param>
        /// <returns></returns>
        private bool Viewable(DisplayCard triggerCard)
        {
            if (isDraging) return false;//拖拽中的卡牌不能查看
            if (triggerCard.IsInDeck) return false; //在牌组的牌不能查看
            if (!triggerCard.IsLocalCard && triggerCard.IsInHand) return false; //对手手牌不能查看
            return true;
        }

        /// <summary>
        /// 处理拖动
        /// </summary>
        private void HandleStartDrag(DisplayCard triggerCard, Action<bool> actions)
        {
            if (!Dragable(triggerCard))
            {
                Debug.LogError("现在不是你的回合");
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
        private void HandleEndDrag(DisplayCard triggerCard, Action<bool> actions)
        {
            if (!isDraging)
            {
                actions?.Invoke(false);
                return;
            }
            isDraging = false;
            actions?.Invoke(true);
        }

        /// <summary>
        /// 处理玩家打出卡牌
        /// 该方法会回到GameFacede中执行
        /// </summary>
        /// <param name="source">触发玩家</param>
        /// <param name="card">触发卡牌</param>
        /// <returns></returns>
        private void HandlePlayedCard(ClientPlayer source, DisplayCard card)
        {
            if (onPlayerPlayCard == null)
            {
                Debug.LogError("出现错误，InputManager没有订阅PlayCard事件！");
                return;
            }
            inputEnable = false; //关闭输入状态
            onPlayerPlayCard?.Invoke(source, card).OnComplete(HandlePlayedCardComplete);
        }

        /// <summary>
        /// 处理玩家打出卡牌结束
        /// 1. 恢复InputManager的输入状态
        /// </summary>
        /// <param name="_">Promise的返回参数，通常为true或false，不过这里是空</param>
        private void HandlePlayedCardComplete(string _)
        {
            inputEnable = true;//恢复输入
        }

        /// <summary>
        /// 处理卡牌是否能被查看
        /// </summary>
        private void HandleView(DisplayCard triggerCard, Action<bool> actions)
        {
            actions?.Invoke(Viewable(triggerCard));
        }

        /// <summary>
        /// 处理卡牌移入放大
        /// </summary>
        private void HanldeZoomIn() { }

        /// <summary>
        /// 处理卡牌移出缩小
        /// </summary>
        private void HandleZoomOut() { }


        /// <summary>
        /// 处理查看卡牌详情
        /// 在左上角显示卡牌详情页面
        /// </summary>
        private void handleViewDetails() { }

        /// <summary>
        /// 处理显示回合结束按钮
        /// </summary>
        private void HandleShowTurnEndBtn()
        {
            TurnEndBtn.gameObject.SetActive(true);
        }

        /// <summary>
        /// 处理隐藏回合结束按钮
        /// </summary>
        private void HandleHideTurnEndBtn()
        {
            TurnEndBtn.gameObject.SetActive(false);
        }

        /// <summary>
        /// 处理回合结束
        /// </summary>
        private void HandleTurnEnd()
        {
            HandleHideTurnEndBtn();
            inputEnable = false; //关闭输入状态
            inputPromise.Resolve(); //结束输入模式
        }

    }
}