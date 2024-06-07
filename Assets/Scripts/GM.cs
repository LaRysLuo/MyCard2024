using System.Collections;
using System.Collections.Generic;
using Larik.CardGame;
using UnityEngine;

public class GM : MonoBehaviour
{
    private GameFacede gameFacede;

    private void Awake()
    {
        gameFacede = new();
        gameFacede.InitGame();

    }

    private void Start()
    {
        gameFacede.BuildDeck(gameFacede.MakeDeck());
        gameFacede.DrawFirstHand().OnComplete((_) =>
        {
            LeanTween.delayedCall(1f, () =>
            {
                gameFacede.StartGame();
            });
        });


    }
}
