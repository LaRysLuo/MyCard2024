using System.Collections;
using System.Collections.Generic;
using Larik.CardGame;
using TMPro;
using UnityEngine;

public class CardDetail : MonoBehaviour
{
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardTypeText;
    [SerializeField] private TMP_Text cardDescText;
    [SerializeField] private Transform cardImageTrans;

    private static CardDetail Instance;

    private static DisplayCard CardPrefab => Resources.Load<DisplayCard>("prefabs/DisplayCardTemplate");

    private static DisplayCard lastCard;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public static void ShowCardDetail(CardInfo cardInfo)
    {
        if (lastCard != null) Destroy(lastCard.gameObject);
        Instance.SetCardInfo(cardInfo);
        Instance.gameObject.SetActive(true);
        lastCard = GameObject.Instantiate<DisplayCard>(CardPrefab, Instance.cardImageTrans);
        lastCard.Init(cardInfo, null, false);
        //让卡牌缩放到跟父元素一致
        float parentWidth = Instance.cardImageTrans.GetComponent<RectTransform>().rect.width;
        // 获取卡牌的原始宽度
        float originalWidth = lastCard.GetComponent<RectTransform>().rect.width;
        // 计算缩放比例
        float scaleX = parentWidth / originalWidth;
        // 设置卡牌的缩放
        lastCard.transform.localScale = new Vector3(scaleX, scaleX, 1);
        // lastCard.transform.localScale = Vector3.one * 2;
    }

    public static void HideCardDetail()
    {
        CanvasGroup canvasGroup = Instance.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvasGroup, 0, 0.2f)
            .setOnComplete(() =>
            {
                Instance.gameObject.SetActive(false);
                canvasGroup.alpha = 1;
                if (lastCard != null) Destroy(lastCard.gameObject);
            });

    }
    private CardInfo cardInfo;

    public void SetCardInfo(CardInfo cardInfo)
    {
        this.cardInfo = cardInfo;
        RefreshCardInfo();
    }

    private void RefreshCardInfo()
    {
        cardNameText.text = cardInfo.name;
        cardTypeText.text = cardInfo.cardType.ToString();
        cardDescText.text = cardInfo.desc;
    }
}
