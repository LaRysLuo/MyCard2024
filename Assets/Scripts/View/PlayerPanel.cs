using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text lifePointText;

    private int value;

    /// <summary>
    /// 刷新生命值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Promise RefreshLifePoint(int value)
    {
        Promise promise = new();
        LeanTween.value(gameObject, this.value, value, 0.5f).setOnUpdate((float val) =>
        {
            lifePointText.text = ((int)val).ToString();
        }).setOnComplete(() =>
        {
            this.value = value;
            promise.Resolve();
        });
        return promise;
    }

}
