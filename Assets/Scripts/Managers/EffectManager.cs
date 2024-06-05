using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Larik.CardGame
{

    public class EffectManager
    {
        private readonly Dictionary<string, Effect> effectDict = new();
        public EffectManager(){
            Debug.Log("效果管理器初始化");

        }

        public void TriggerEffect(ClientPlayer source,DisplayCard card){
            Debug.Log("触发效果");
        }
    }
}

