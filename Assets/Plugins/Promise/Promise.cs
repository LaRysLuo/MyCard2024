using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Promise
{
    /// <summary>
    /// 按顺序执行Promise
    /// </summary>
    /// <returns></returns>
    public static SequentialCaller SequentialCall(List<Func<Promise>> actions, float interval = 0, bool isFailBreak = false)
    {
        return new SequentialCaller(actions, interval, isFailBreak);
    }

    // public static SequentialCaller SequentialCall2(List<Action<Promise>> actions, float interval = 0, bool isFailBreak = false)
    // {
    //     return new SequentialCaller(actions, interval, isFailBreak);
    // }



    private Action<string> onComplete;
    private Action<string> onCatch;

    private bool isResolve;

    public Promise Resolve(string result = null)
    {
        isResolve = true;
        LeanTween.delayedCall(0.01f, () =>
        {
            onComplete?.Invoke(result);
        });
        return this;
    }

    public Promise Reject(string result = null)
    {
        isResolve = false;
        LeanTween.delayedCall(0.01f, () =>
        {
            onCatch?.Invoke(result);
        });
        return this;
    }

    public Promise OnComplete(Action<string> onComplete)
    {
        this.onComplete = onComplete;
        return this;
    }

    public Promise Catch(Action<string> onCatch)
    {
        if (!isResolve)
        {
            this.onCatch = onCatch;
        }
        return this;
    }


}
