using UnityEngine;
using System.Collections.Generic;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>
    /// At Start, moves every CharacterUI to the AllyUI matching its combatant's RoomIndex.
    /// </summary>
    public class UIRoomPlacer : MonoBehaviour
    {
        private void Start()
        {
            var canvas = GetComponentInParent<Canvas>()?.transform
                      ?? FindAnyObjectByType<Canvas>()?.transform;
            if (canvas == null) return;

            // Build AllyUI map: roomIndex -> AllyUI transform
            var allyUIMap = new Dictionary<int, Transform>();
            int i = 1;
            while (true)
            {
                var panel = canvas.Find($"PanelRoom{i}");
                if (panel == null) break;
                var ally = panel.Find("AllyUI");
                if (ally != null) allyUIMap[i - 1] = ally;
                i++;
            }

            // Collect all non-enemy combatants sorted by RoomIndex then by combatant order
            var all = FindObjectsByType<Combatant>(FindObjectsInactive.Include);
            System.Array.Sort(all, (a, b) => a.RoomIndex.CompareTo(b.RoomIndex));

            var allBinders = FindObjectsByType<CharacterUIAutoBind>(FindObjectsInactive.Include);

            foreach (var combatant in all)
            {
                if (combatant.IsEnemy) continue;
                if (!allyUIMap.TryGetValue(combatant.RoomIndex, out var targetAllyUI)) continue;

                CharacterUIAutoBind match = null;
                foreach (var b in allBinders)
                    if (b.GetCharacterBehaviour() == combatant) { match = b; break; }
                if (match == null) continue;

                // Reparent to the correct AllyUI
                match.transform.SetParent(targetAllyUI, false);
                match.gameObject.SetActive(true);
                targetAllyUI.gameObject.SetActive(true);
            }

            // Hide AllyUI that ended up with no CharacterUIBinder children
            foreach (var kvp in allyUIMap)
            {
                var ally = kvp.Value;
                bool hasChild = ally.GetComponentInChildren<CharacterUIBinder>(true) != null;
                ally.gameObject.SetActive(hasChild);
            }
        }
    }
}
