using UnityEngine;
using UnityEngine.UI;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class RoomMapUI : MonoBehaviour
    {
        [SerializeField] private RoomNavigator roomNavigator;
        [SerializeField] private Transform mapSection;

        private static readonly Color ActiveColor   = new Color(1f, 0.85f, 0f);
        private static readonly Color InactiveColor = new Color(0.4f, 0.4f, 0.4f);

        private Button[]    _buttons;
        private Transform[] _dotContainers;
        private int[]       _combatantRoom; // index = combatantIndex, value = roomIndex

        private void Start()
        {
            var root = mapSection != null ? mapSection : transform;

            // Discover nodes dynamically
            var buttonList    = new System.Collections.Generic.List<Button>();
            var containerList = new System.Collections.Generic.List<Transform>();
            int n = 0;
            while (true)
            {
                var t = root.Find($"RoomNode_{n}");
                if (t == null) break;
                buttonList.Add(t.GetComponent<Button>());
                containerList.Add(t.Find("CombatantDots"));
                n++;
            }
            _buttons       = buttonList.ToArray();
            _dotContainers = containerList.ToArray();

            for (int i = 0; i < _buttons.Length; i++)
            {
                int captured = i;
                if (_buttons[i] != null)
                    _buttons[i].onClick.AddListener(() => roomNavigator?.NavigateTo(captured));
            }

            // Seed combatant positions from the actual Combatant components
            var allies = FindObjectsByType<Combatant>(FindObjectsInactive.Include);
            var allyList = new System.Collections.Generic.List<Combatant>();
            foreach (var c in allies) if (!c.IsEnemy) allyList.Add(c);
            allyList.Sort((a, b) => a.RoomIndex.CompareTo(b.RoomIndex));

            _combatantRoom = new int[allyList.Count];
            for (int i = 0; i < allyList.Count; i++)
                _combatantRoom[i] = allyList[i].RoomIndex;

            RefreshDots();
            SetActiveRoom(roomNavigator != null ? roomNavigator.CurrentRoomIndex : 0);
        }

        public void SetActiveRoom(int index)
        {
            if (_buttons == null) return;
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i] == null) continue;
                var img = _buttons[i].GetComponent<Image>();
                if (img != null)
                    img.color = (i == index) ? ActiveColor : InactiveColor;
            }
        }

        public void SetCombatantRoom(int combatantIndex, int roomIndex)
        {
            if (_combatantRoom == null || combatantIndex < 0 || combatantIndex >= _combatantRoom.Length) return;
            _combatantRoom[combatantIndex] = roomIndex;
            RefreshDots();
        }

        private void RefreshDots()
        {
            if (_dotContainers == null || _combatantRoom == null) return;
            for (int node = 0; node < _dotContainers.Length; node++)
            {
                var container = _dotContainers[node];
                if (container == null) continue;
                for (int c = 0; c < _combatantRoom.Length; c++)
                {
                    var dot = container.Find($"Dot_C{c + 1}");
                    if (dot != null)
                        dot.gameObject.SetActive(_combatantRoom[c] == node);
                }
            }
        }
    }
}
