using System;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Larik.CardGame
{
    public class DisplayCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public CardInfo cardInfo;

        private Action<string, DisplayCard, Action<bool>> onPlayerAction;
        private RectTransform Parent => transform.parent.GetComponent<RectTransform>();

        public void Init(CardInfo cardInfo, Action<string, DisplayCard, Action<bool>> onPlayerAction)
        {
            this.cardInfo = cardInfo;
            this.onPlayerAction = onPlayerAction;
            gameObject.name = cardInfo.name;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

            onPlayerAction?.Invoke("onBeginDrag", this, (isEnable) =>
            {
                Debug.Log(111);
                if (!isEnable) return;


            });
        }

        public void OnDrag(PointerEventData eventData)
        {
            onPlayerAction?.Invoke("onDrag", this, (isEnable) =>
            {
                if (!isEnable) return;
                GetComponent<RectTransform>().anchoredPosition += eventData.delta / Parent.localScale.x;
            });

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onPlayerAction?.Invoke("onEndDrag", this, (isEnable) =>
            {
                if (!isEnable) return;
                // transform.SetSiblingIndex(originalSiblingIndex); //设置优先级到初始位
                transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;//开启Layout
            });
        }
    }
}
