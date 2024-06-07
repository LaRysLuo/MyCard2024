using System;
using System.Collections;
using System.Collections.Generic;
using Larik.CardGame;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasePanel : MonoBehaviour, IPointerClickHandler
{
    private Action<string, DisplayCard, Action<bool>> onPlayerAction;
    public void AddEventListener(Action<string, DisplayCard, Action<bool>> onPlayerAction)
    {
        this.onPlayerAction = onPlayerAction;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        //点击左键
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CardDetail.HideCardDetail();
        }
    }
}
