using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Larik.CardGame
{
    public static class AssetManager
    {
        public static Transform BattleGrid => GameObject.Find("BattleField").transform;

        public static GameObject PlayerViewPrefab => Resources.Load<GameObject>("prefabs/PlayerViewTemplate");

        public static DisplayCard DisplayCardPrefab => Resources.Load<DisplayCard>("prefabs/DisplayCardTemplate");

    }
}
