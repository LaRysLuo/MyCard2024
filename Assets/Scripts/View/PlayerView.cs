using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Larik.CardGame
{
    public class PlayerView : MonoBehaviour
    {
        public PlayerPanel PlayerPanel => GetComponent<PlayerPanel>();
        /// <summary>
        /// 牌组
        /// </summary>
        /// <typeparam name="Deck"></typeparam>
        /// <returns></returns>
        public Deck Deck => transform.Find("Deck").GetComponent<Deck>();

        /// <summary>
        /// 场地
        /// </summary>
        /// <typeparam name="Field"></typeparam>
        /// <returns></returns>
        public Field Field => transform.Find("Field").GetComponent<Field>();

        /// <summary>
        /// 手牌
        /// </summary>
        /// <typeparam name="Hand"></typeparam>
        /// <returns></returns>
        public Hand Hand => transform.Find("Hand").GetComponent<Hand>();

        /// <summary>
        /// 弃牌区
        /// </summary>
        /// <typeparam name="Discard"></typeparam>
        /// <returns></returns>
        public Discard Discard => transform.Find("Discard").GetComponent<Discard>();
    }

}

