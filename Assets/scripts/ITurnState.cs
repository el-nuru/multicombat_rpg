namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public interface ITurnState
    {
        void Enter(TurnAction context);
        void Exit(TurnAction context);
        void OnFight(TurnAction context);
        void OnRun(TurnAction context);
        void Update(TurnAction context);
    }
}
