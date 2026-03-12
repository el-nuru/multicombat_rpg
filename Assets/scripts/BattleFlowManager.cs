using UnityEngine;
using UnityEngine.UI;
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

        private Dictionary<int, RoomBattleManager> _roomMap;

        private void Start()
        {
            foreach (var btn in FindObjectsByType<Button>(FindObjectsInactive.Include))
            {
                if (btn.name == "FightBtn") btn.onClick.AddListener(OnFightButtonPressed);
                if (btn.name == "RunBtn")   btn.onClick.AddListener(OnRunButtonPressed);
            }

            var rooms = FindObjectsByType<RoomBattleManager>(FindObjectsInactive.Include);
            _roomMap = new Dictionary<int, RoomBattleManager>();
            foreach (var r in rooms)
                _roomMap[r.RoomIndex] = r;

            var combatants = FindObjectsByType<Combatant>(FindObjectsInactive.Include);
            foreach (var c in combatants)
                if (_roomMap.TryGetValue(c.RoomIndex, out var room))
                    room.RegisterCombatant(c);
        }

        public void SetActiveRoomIndex(int index) => activeRoomIndex = index;

        public void OnFightButtonPressed() => ActiveRoom?.TryPlayerAction(PlayerAction.Fight);
        public void OnRunButtonPressed()  => ActiveRoom?.TryPlayerAction(PlayerAction.Run);

        private RoomBattleManager ActiveRoom =>
            _roomMap != null && _roomMap.TryGetValue(activeRoomIndex, out var r) ? r : null;
    }
}
