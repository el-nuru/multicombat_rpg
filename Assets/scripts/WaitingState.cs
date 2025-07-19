using UnityEngine;
namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class WaitingState : ITurnState
    {
        public void Enter(TurnAction context)
        {
            // Player cannot act while waiting
            // Start the time bar fill
            context.StartTimeBar(context.fillTimeSeconds);
        }

        public void Exit(TurnAction context) { }

        public void OnFight(TurnAction context) { }
        public void OnRun(TurnAction context) { }
        public void Update(TurnAction context) { }
    }
}
