using System;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Larik.CardGame
{
    public class DisplayCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public CardInfo cardInfo;

        [SerializeField] private TMP_Text cardName;
        [SerializeField] private TMP_Text cardTypeName;
        [SerializeField] private TMP_Text contentText;

        [SerializeField] private Transform fontTrans;
        [SerializeField] private Transform backTrans;


        private bool isBack = false;

        private Action<string, DisplayCard, Action<bool>> onPlayerAction;
        private RectTransform Parent => transform.parent.GetComponent<RectTransform>();

        private HorizontalLayoutGroup LayoutGroup => transform.parent.GetComponent<HorizontalLayoutGroup>();

        public Transform Controller => transform.parent.parent;

        public bool IsLocalCard => Controller.gameObject.name == "self";

        public bool IsInDeck => transform.parent.gameObject.name == "Deck";

        public bool IsInHand => transform.parent.gameObject.name == "Hand";


        public void Init(CardInfo cardInfo, Action<string, DisplayCard, Action<bool>> onPlayerAction)
        {
            this.cardInfo = cardInfo;
            this.onPlayerAction = onPlayerAction;
            gameObject.name = cardInfo.name;
            RefreshCardInfo();
            InitLongHoldEvent();
        }

        /// <summary>
        /// 注册长按事件
        /// </summary>
        private void InitLongHoldEvent()
        {
            GetComponent<LongHoldEvent>().AddEventListener(OnLongHoldHappened);
        }

        /// <summary>
        /// 刷新卡牌信息
        /// </summary>
        public void RefreshCardInfo()
        {
            cardName.text = cardInfo.name;
            cardTypeName.text = cardInfo.cardType.ToString();
            contentText.text = cardInfo.desc;

            onPlayerAction?.Invoke("onView", this, (isEnable) =>
            {
                Debug.Log(123);
                SetBackState(!isEnable);
            });
        }

        /// <summary>
        /// 设置卡面状态
        /// </summary>
        /// <param name="isBack">是否为背面</param>
        private void SetBackState(bool isBack)
        {
            this.isBack = isBack;
            fontTrans.gameObject.SetActive(!isBack);
            backTrans.gameObject.SetActive(isBack);
        }

        #region 拖动Drag



        /// <summary>
        /// 开始拖动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {

            onPlayerAction?.Invoke("onBeginDrag", this, (isEnable) =>
            {
                Debug.Log(111);
                if (!isEnable) return;


            });
        }
        /// <summary>
        /// 拖动中
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            onPlayerAction?.Invoke("onDrag", this, (isEnable) =>
            {
                if (!isEnable) return;
                GetComponent<RectTransform>().anchoredPosition += eventData.delta / Parent.localScale.x;
            });

        }

        /// <summary>
        /// 停止拖动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            onPlayerAction?.Invoke("onEndDrag", this, (isEnable) =>
            {
                if (!isEnable) return;
                // transform.SetSiblingIndex(originalSiblingIndex); //设置优先级到初始位
                transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;//开启Layout
            });
        }


        /// <summary>
        /// 卡牌拖动到出牌区域并松开了鼠标
        /// </summary>
        public void OnDrop() { }

        /// <summary>
        /// 当长按事件触发了
        /// </summary>
        private void OnLongHoldHappened() { }

        private Vector2 transOriginLocalScale;
        private int originalSiblingIndex;
        private bool isZoom = false;
        public void OnPointerEnter(PointerEventData eventData)
        {
            onPlayerAction?.Invoke("onView", this, (isEnable) =>
            {
                Debug.Log("onView:" + isEnable);
                if (!isEnable) return;
                //卡牌放大10%
                LayoutGroup.enabled = false;
                transOriginLocalScale = transform.localScale;
                originalSiblingIndex = transform.GetSiblingIndex();
                transform.localScale = Vector3.one * 1.1f;
                transform.SetAsLastSibling();
                isZoom = true;
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isZoom) return;
            transform.localScale = transOriginLocalScale;
            transform.SetSiblingIndex(originalSiblingIndex);
            LayoutGroup.enabled = true;
        }

        #endregion



    }
}
