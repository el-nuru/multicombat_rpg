using UnityEngine;
using Object = UnityEngine.Object;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class UIRoomPlacer : MonoBehaviour
    {
        private void Start()
        {
            var canvas = GetComponentInParent<Canvas>()?.transform
                      ?? FindAnyObjectByType<Canvas>()?.transform;
            if (canvas == null) { Debug.LogError("[UIRoomPlacer] canvas null"); return; }

            var allyUIPerRoom = SceneQueries.FindAllyUIPerRoom(canvas);
            var allies        = SceneQueries.FindAllAllies();

            var binders = Object.FindObjectsByType<CharacterUIBinder>(FindObjectsInactive.Include);
            foreach (var b in binders)
                b.gameObject.SetActive(false);

            foreach (var combatant in allies)
            {
                int room = combatant.RoomIndex;
                var ui = SceneQueries.FindCharacterUI(combatant);
                if (room < 0 || room >= allyUIPerRoom.Length || allyUIPerRoom[room] == null) continue;
                if (ui == null) continue;

                var targetAllyUI = allyUIPerRoom[room];
                ui.transform.SetParent(targetAllyUI, false);
                ui.gameObject.SetActive(true);
                targetAllyUI.gameObject.SetActive(true);
            }

            foreach (var allyUI in allyUIPerRoom)
            {
                if (allyUI == null) continue;
                bool show = allyUI.GetComponentInChildren<CharacterUIBinder>(true) != null;
                allyUI.gameObject.SetActive(show);
            }
        }
    }
}
