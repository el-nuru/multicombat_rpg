using System.Collections.Generic;
using UnityEngine; // MonoBehaviour, SerializeField

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>
    /// Manages ATB combat for a single room. One instance per room.
    /// </summary>
    public class RoomBattleManager : MonoBehaviour
    {
        [Header("Room")]
        [SerializeField] private int _roomIndex;
        public int RoomIndex => _roomIndex;

        private readonly List<Combatant>       _combatants   = new();
        private readonly Queue<ICombatCommand> _commandQueue = new();
        private readonly List<Combatant>       _queryBuffer  = new();
        private readonly StateMachine          _fsm          = new();

        private void Awake() => _fsm.ChangeState(new CombatActiveState(this));

        public void RegisterCombatant(Combatant c)   => _combatants.Add(c);
        public void UnregisterCombatant(Combatant c) => _combatants.Remove(c);

        private void Update() => _fsm.Update();

        // Llamado por CombatActiveState cada frame
        public void ProcessTick()
        {
            ProcessEnemyTurns();

            if (_commandQueue.Count > 0)
                _commandQueue.Dequeue().Execute();

            CheckTransitions();
        }

        private void CheckTransitions()
        {
            if (!HasLivingCombatants(isEnemy: false))
                _fsm.ChangeState(new CombatDefeatState(this));
            else if (!HasLivingCombatants(isEnemy: true))
                _fsm.ChangeState(new CombatVictoryState(this));
        }

        private void ProcessEnemyTurns()
        {
            foreach (var c in _combatants)
            {
                if (!c.IsEnemy || c.ElapsedTime < c.FillTime) continue;
                var targets = GetLivingCombatants(isEnemy: false);
                if (targets.Count == 0) continue;
                var cmd = c.Strategy.DecideAction(c, targets);
                if (cmd != null) _commandQueue.Enqueue(cmd);
                c.ElapsedTime = 0f;
            }
        }

        public bool TryPlayerAction(PlayerAction action) =>
            TryPlayerAction(action, GetReadyAlly());

        public bool TryPlayerAction(PlayerAction action, Combatant actor)
        {
            if (actor == null || actor.IsEnemy || !actor.IsAlive() || actor.ElapsedTime < actor.FillTime)
                return false;

            var targets = GetLivingCombatants(isEnemy: true);
            ICombatCommand cmd = action == PlayerAction.Fight && targets.Count > 0
                ? new AttackCommand(actor, targets[Random.Range(0, targets.Count)])
                : new RunCommand(actor);

            _commandQueue.Enqueue(cmd);
            actor.ElapsedTime = 0f;
            return true;
        }

        private Combatant GetReadyAlly()
        {
            foreach (var c in _combatants)
                if (!c.IsEnemy && c.IsAlive() && c.ElapsedTime >= c.FillTime) return c;
            return null;
        }

        private bool HasLivingCombatants(bool isEnemy)
        {
            foreach (var c in _combatants)
                if (c.IsEnemy == isEnemy && c.IsAlive()) return true;
            return false;
        }

        private List<Combatant> GetLivingCombatants(bool isEnemy)
        {
            _queryBuffer.Clear();
            foreach (var c in _combatants)
                if (c.IsEnemy == isEnemy && c.IsAlive()) _queryBuffer.Add(c);
            return _queryBuffer;
        }
    }

    public enum PlayerAction { Fight, Run }
}
