using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Larik.CardGame
{
    public class UIManager : MonoBehaviour
    {
        // private BasePanel BasePanel => GetComponent<BasePanel>();


        // public void AddEventListener(Action<string , ClientPlayer, DisplayCard , Action<bool>> onPlayerAction)
        // {
        //     BasePanel.AddEventListener(onPlayerAction);
        // }

        public List<PlayerPanel> playerPanels = new();

        public Button turnEndBtn;


        private void Awake() {
            turnEndBtn.gameObject.SetActive(false);
        }
    }

}

