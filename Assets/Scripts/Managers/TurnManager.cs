using System;

namespace Larik.CardGame
{
    public class TurnManager
    {
        public int turns;
        public int current;

        private Func<int, int, Promise> onTurnStart;

        public void AddEventListener(Func<int, int, Promise> onTurnStart)
        {
            this.onTurnStart = onTurnStart;
        }

        public void StartGame()
        {
            turns = 0;
        }

        public void NextTurn()
        {
            turns++;
            current = turns % 2;
            onTurnStart?.Invoke(current, turns).OnComplete((_) => NextTurn());
        }
    }
}