using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>
    /// Mueve un Combatant por el RoomGraph habitación por habitación.
    /// Cada paso tarda SecondsPerRoom segundos.
    /// </summary>
    public class AgentMover : MonoBehaviour
    {
        public float SecondsPerRoom = 5f;

        private RoomGraph         _graph;
        private BattleFlowManager _battleFlow;
        private RoomMapUI         _roomMapUI;

        private readonly Dictionary<Combatant, Coroutine> _activeMovements = new();

        private void Start()
        {
            _graph      = FindAnyObjectByType<RoomGraph>();
            _battleFlow = FindAnyObjectByType<BattleFlowManager>();
            _roomMapUI  = FindAnyObjectByType<RoomMapUI>();
        }

        /// <summary>
        /// Ordena al combatant moverse a targetRoom.
        /// Si ya está moviéndose, cancela el movimiento anterior.
        /// </summary>
        public void MoveTo(Combatant combatant, int targetRoom)
        {
            if (combatant == null || _graph == null) return;

            if (_activeMovements.TryGetValue(combatant, out var existing) && existing != null)
                StopCoroutine(existing);

            var path = _graph.GetPath(combatant.RoomIndex, targetRoom);
            if (path == null || path.Count == 0) return;

            _activeMovements[combatant] = StartCoroutine(MoveAlongPath(combatant, path));
        }

        public void CancelMovement(Combatant combatant)
        {
            if (_activeMovements.TryGetValue(combatant, out var c) && c != null)
                StopCoroutine(c);
            _activeMovements.Remove(combatant);
        }

        public bool IsMoving(Combatant combatant) =>
            _activeMovements.TryGetValue(combatant, out var c) && c != null;

        private IEnumerator MoveAlongPath(Combatant combatant, List<int> path)
        {
            int combatantIndex = SceneQueries.FindAllAllies().IndexOf(combatant);

            foreach (int nextRoom in path)
            {
                yield return new WaitForSeconds(SecondsPerRoom);

                _battleFlow?.MoveToRoom(combatant, nextRoom);
                _roomMapUI?.SetCombatantRoom(combatantIndex, nextRoom);
            }

            _activeMovements.Remove(combatant);
        }
    }
}
