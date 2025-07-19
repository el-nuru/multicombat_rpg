using UnityEngine;
namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class ReadyState : ITurnState
    {
        public void Enter(TurnAction turn) { }
        public void Exit(TurnAction turn) { }

        public void OnFight(TurnAction turn)
        {
            turn.PrintFightMessage();
            turn.ChangeState(new WaitingState());
        }

        public void OnRun(TurnAction turn)
        {
            turn.PrintRunMessage();
            turn.ChangeState(new WaitingState());
        }

        public void Update(TurnAction turn) { }
    }
}