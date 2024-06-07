using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Larik.CardGame
{
    public abstract class CardGrid : MonoBehaviour
    {
        public Transform EndPoint => transform.Find("EndPoint");
        public List<DisplayCard> CardList => GetComponentsInChildren<DisplayCard>().ToList();

        public bool Contains(DisplayCard card)
        {
            return CardList.Contains(card);
        }

        public void Add(DisplayCard card)
        {
            card.transform.SetParent(transform);

            // card.Init();
            card.RefreshCardInfo();
            card.transform.localPosition = Vector3.zero;
            if (EndPoint) EndPoint.SetAsLastSibling();
        }
    }

}

