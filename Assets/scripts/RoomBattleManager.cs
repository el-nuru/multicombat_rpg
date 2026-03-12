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

        public void RegisterCombatant(Combatant c) => _combatants.Add(c);

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

        public bool TryPlayerAction(PlayerAction action)
        {
            var ready = GetReadyAlly();
            if (ready == null) return false;

            var target = GetLivingCombatants(isEnemy: true);
            ICombatCommand cmd = action == PlayerAction.Fight && target.Count > 0
                ? new AttackCommand(ready, PickRandom(target))
                : new RunCommand(ready);

            _commandQueue.Enqueue(cmd);
            ready.ElapsedTime = 0f;
            return true;
        }

        public bool IsAnyAllyReady() => GetReadyAlly() != null;

        private Combatant GetReadyAlly()
        {
            foreach (var c in _combatants)
                if (!c.IsEnemy && c.IsAlive() && c.ElapsedTime >= c.FillTime) return c;
            return null;
        }

        private List<Combatant> GetLivingCombatants(bool isEnemy)
        {
            var list = new List<Combatant>();
            foreach (var c in _combatants)
                if (c.IsEnemy == isEnemy && c.IsAlive()) list.Add(c);
            return list;
        }

        private static Combatant PickRandom(List<Combatant> list) =>
            list[Random.Range(0, list.Count)];
    }

    public enum PlayerAction { Fight, Run }
}
