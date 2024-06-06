using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongHoldEvent : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private bool pointerDown = false;
    private float pointerDownTimer = 0;

    [SerializeField] private readonly float requiredHoldTime = 1;

    private Action onLongHoldEvent;

    public void AddEventListener(Action onLongHoldEvent)
    {
        this.onLongHoldEvent = onLongHoldEvent;
    }

    private void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            // Debug.Log(pointerDownTimer);
            if (pointerDownTimer >= requiredHoldTime)
            {
                Debug.Log("长按触发了");
                onLongHoldEvent?.Invoke();
                Reset();
            }
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }

    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
    }
}
