using UnityEngine;
using System.Collections.Generic;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>
    /// Coordinates all room battles. Routes player input to the active room's RoomBattleManager.
    /// Discovers RoomBattleManagers and Combatants automatically on Start.
    /// </summary>
    public class BattleFlowManager : MonoBehaviour
    {
        public int activeRoomIndex = 0;

        private RoomBattleManager[] _rooms;

        private void Start()
        {
            _rooms = FindObjectsByType<RoomBattleManager>(FindObjectsInactive.Include);

            var combatants = FindObjectsByType<Combatant>(FindObjectsInactive.Include);
            var roomMap = new Dictionary<int, RoomBattleManager>();
            foreach (var r in _rooms)
                roomMap[r.RoomIndex] = r;

            foreach (var c in combatants)
                if (roomMap.TryGetValue(c.RoomIndex, out var room))
                    room.RegisterCombatant(c);
        }

        public void SetActiveRoomIndex(int index) => activeRoomIndex = index;

        public void OnFightButtonPressed() => ActiveRoom?.TryPlayerAction(PlayerAction.Fight);
        public void OnRunButtonPressed()  => ActiveRoom?.TryPlayerAction(PlayerAction.Run);

        private RoomBattleManager ActiveRoom => GetRoom(activeRoomIndex);

        private RoomBattleManager GetRoom(int index)
        {
            if (_rooms == null) return null;
            foreach (var r in _rooms)
                if (r.RoomIndex == index) return r;
            return null;
        }
    }
}
