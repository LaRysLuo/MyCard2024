using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Larik.CardGame
{
    public class Field : CardGrid, IDropHandler
    {
        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            Debug.Log("OnDrop");
            if (eventData.pointerDrag != null)
            {
                if (eventData.pointerDrag.TryGetComponent<DisplayCard>(out var card))
                {

                }
            }
        }
    }

}

