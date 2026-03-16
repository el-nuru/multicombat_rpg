using System.Collections.Generic;
using UnityEngine;

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

        private readonly List<Combatant> _combatants = new();
        private readonly Queue<ICombatCommand> _commandQueue = new();
        private readonly List<Combatant> _queryBuffer = new();

        public void RegisterCombatant(Combatant c) => _combatants.Add(c);
        public void UnregisterCombatant(Combatant c) => _combatants.Remove(c);

        private void Update()
        {
            ProcessEnemyTurns();
            if (_commandQueue.Count > 0)
                _commandQueue.Dequeue().Execute();
        }

        private void ProcessEnemyTurns()
        {
            foreach (var c in _combatants)
            {
                if (!c.IsEnemy || c.ElapsedTime < c.FillTime) continue;
                var targets = GetLivingCombatants(isEnemy: false);
                if (targets.Count == 0) continue;
                _commandQueue.Enqueue(new AttackCommand(c, PickRandom(targets)));
                c.ElapsedTime = 0f;
            }
        }

        public bool TryPlayerAction(PlayerAction action) =>
            TryPlayerAction(action, GetReadyAlly());

        public bool TryPlayerAction(PlayerAction action, Combatant actor)
        {
            if (actor == null || actor.IsEnemy || !actor.IsAlive() || actor.ElapsedTime < actor.FillTime)
                return false;

            var target = GetLivingCombatants(isEnemy: true);
            ICombatCommand cmd = action == PlayerAction.Fight && target.Count > 0
                ? new AttackCommand(actor, PickRandom(target))
                : new RunCommand(actor);

            _commandQueue.Enqueue(cmd);
            actor.ElapsedTime = 0f;
            return true;
        }

        public bool IsAnyAllyReady() => GetReadyAlly() != null;
        public bool HasAlly() { foreach (var c in _combatants) if (!c.IsEnemy && c.IsAlive()) return true; return false; }

        private Combatant GetReadyAlly()
        {
            foreach (var c in _combatants)
                if (!c.IsEnemy && c.IsAlive() && c.ElapsedTime >= c.FillTime) return c;
            return null;
        }

        private List<Combatant> GetLivingCombatants(bool isEnemy)
        {
            _queryBuffer.Clear();
            foreach (var c in _combatants)
                if (c.IsEnemy == isEnemy && c.IsAlive()) _queryBuffer.Add(c);
            return _queryBuffer;
        }

        private static Combatant PickRandom(List<Combatant> list) =>
            list[Random.Range(0, list.Count)];
    }

    public enum PlayerAction { Fight, Run }
}
