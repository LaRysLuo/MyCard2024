using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequentialCaller
{
    private readonly Queue<Func<Promise>> actions = new();
    private readonly Promise resultPromise = new();

    private readonly float interval;
    private readonly bool isFailBreak;

    public SequentialCaller(List<Func<Promise>> actions, float interval = 0, bool isFailBreak = false)
    {
        foreach (var action in actions)
        {
            this.actions.Enqueue(action);
        }
        this.interval = interval;
        this.isFailBreak = isFailBreak;
        CallNext();
    }

    private void CallNext()
    {
        // Debug.LogWarning("开始执行SequentialCaller");
        if (actions.Count > 0)
        {
            var action = actions.Dequeue();
            action?.Invoke().OnComplete((result) =>
            {
                //  Debug.LogWarning("SequentialCaller执行完一段");
                if (isFailBreak && result == "break")
                {
                    // Debug.LogWarning("中断执行SequentialCaller");
                    resultPromise.Reject("break");
                    actions.Clear();
                    return;
                }
                LeanTween.delayedCall(interval, () =>
                {
                    CallNext();
                });
            });
        }
        else
        {
            // Debug.LogWarning("SequentialCaller执行完毕");
            resultPromise.Resolve();
        }
    }

    public Promise OnComplete(Action<string> onComplete)
    {
        return resultPromise.OnComplete(onComplete);
    }
}
