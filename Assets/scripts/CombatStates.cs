namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>Combate activo: timers corriendo, procesando comandos.</summary>
    public class CombatActiveState : IState
    {
        private readonly RoomBattleManager _room;
        public CombatActiveState(RoomBattleManager room) => _room = room;

        public void OnEnter()  { }
        public void OnExit()   { }
        public void OnUpdate() => _room.ProcessTick();
    }

    /// <summary>Sin aliados vivos: combate terminado en derrota.</summary>
    public class CombatDefeatState : IState
    {
        private readonly RoomBattleManager _room;
        public CombatDefeatState(RoomBattleManager room) => _room = room;

        public void OnEnter()  => CombatEvents.RoomDefeated(_room.RoomIndex);
        public void OnExit()   { }
        public void OnUpdate() { }
    }

    /// <summary>Sin enemigos vivos: combate terminado en victoria.</summary>
    public class CombatVictoryState : IState
    {
        private readonly RoomBattleManager _room;
        public CombatVictoryState(RoomBattleManager room) => _room = room;

        public void OnEnter()  => CombatEvents.RoomCleared(_room.RoomIndex);
        public void OnExit()   { }
        public void OnUpdate() { }
    }
}
