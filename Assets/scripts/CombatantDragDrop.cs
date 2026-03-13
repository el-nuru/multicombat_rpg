using UnityEngine;
using UnityEngine.EventSystems;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class CombatantDragDrop : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private int combatantIndex;
        [SerializeField] private RoomMapUI roomMapUI;
        [SerializeField] private Transform mapSection;

        private RectTransform _rect;
        private CanvasGroup   _canvasGroup;
        private Vector2       _originalPos;
        private Canvas        _canvas;
        private Combatant     _combatant;
        private BattleFlowManager _battleFlow;
        private Transform     _characterUI;
        private Transform     _colorIndicator; // CombatantColor sibling in PanelRoom
        private Transform[]   _allyUIPerRoom;
        private Transform[]   _panelPerRoom;
        private bool          _initialized;

        private void OnEnable()
        {
            if (_initialized) return;
            _initialized = true;

            _rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            _canvas = GetComponentInParent<Canvas>();

            _battleFlow = FindFirstObjectByType<BattleFlowManager>();

            // Find combatant by index (non-enemy, sorted by RoomIndex)
            var all = FindObjectsByType<Combatant>(FindObjectsInactive.Include);
            System.Array.Sort(all, (a, b) => a.RoomIndex.CompareTo(b.RoomIndex));
            int found = 0;
            foreach (var c in all)
            {
                if (c.IsEnemy) continue;
                if (found == combatantIndex) { _combatant = c; break; }
                found++;
            }

            // Find CharacterUI by matching characterBehaviour == _combatant
            if (_combatant != null)
            {
                var allBinders = FindObjectsByType<CharacterUIAutoBind>(FindObjectsInactive.Include);
                foreach (var b in allBinders)
                    if (b.GetCharacterBehaviour() == _combatant) { _characterUI = b.transform; break; }
            }

            // Find AllyUI and PanelRoom transforms dynamically
            var canvas = _canvas != null ? _canvas.transform : null;
            if (canvas != null)
            {
                var allyList  = new System.Collections.Generic.List<Transform>();
                var panelList = new System.Collections.Generic.List<Transform>();
                int i = 1;
                while (true)
                {
                    var panel = canvas.Find($"PanelRoom{i}");
                    if (panel == null) break;
                    allyList.Add(panel.Find("AllyUI"));
                    panelList.Add(panel);
                    i++;
                }
                _allyUIPerRoom = allyList.ToArray();
                _panelPerRoom  = panelList.ToArray();
            }

            // Find this combatant's color indicator
            if (_combatant != null && _panelPerRoom != null)
            {
                int room = _combatant.RoomIndex;
                if (room < _panelPerRoom.Length && _panelPerRoom[room] != null)
                    _colorIndicator = _panelPerRoom[room].Find("CombatantColor");
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPos = _rect.anchoredPosition;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.7f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_canvas != null) _rect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
            _rect.anchoredPosition = _originalPos;

            int targetRoom = GetRoomNodeUnderPointer(eventData);
            if (targetRoom < 0 || _combatant == null) return;

            int currentRoom = _combatant.RoomIndex;
            if (currentRoom == targetRoom) return;

            _battleFlow?.MoveToRoom(_combatant, targetRoom);

            // Move CharacterUI to target AllyUI
            if (_characterUI != null && _allyUIPerRoom != null &&
                targetRoom < _allyUIPerRoom.Length && _allyUIPerRoom[targetRoom] != null)
            {
                _allyUIPerRoom[targetRoom].gameObject.SetActive(true);
                _characterUI.SetParent(_allyUIPerRoom[targetRoom], false);
                _characterUI.gameObject.SetActive(true);
                RefreshAllyUIStack(_allyUIPerRoom[targetRoom]);
            }

            // Move color indicator to target PanelRoom
            if (_colorIndicator != null && _panelPerRoom != null &&
                targetRoom < _panelPerRoom.Length && _panelPerRoom[targetRoom] != null)
            {
                _colorIndicator.SetParent(_panelPerRoom[targetRoom], false);
            }

            // Hide source AllyUI if empty; restack if not
            if (_allyUIPerRoom != null && currentRoom < _allyUIPerRoom.Length && _allyUIPerRoom[currentRoom] != null)
            {
                var srcAlly = _allyUIPerRoom[currentRoom];
                bool hasAlly = srcAlly.GetComponentInChildren<CharacterUIBinder>(true) != null;
                srcAlly.gameObject.SetActive(hasAlly);
                if (hasAlly) RefreshAllyUIStack(srcAlly);
            }

            roomMapUI?.SetCombatantRoom(combatantIndex, targetRoom);
        }

        private const float StackOffsetLocalY = -22000f;

        private static void RefreshAllyUIStack(Transform allyUI)
        {
            int slot = 0;
            for (int i = 0; i < allyUI.childCount; i++)
            {
                var child = allyUI.GetChild(i);
                if (!child.gameObject.activeSelf) continue;
                if (child.GetComponent<CharacterUIBinder>() == null) continue;
                var lp = child.localPosition;
                lp.y = slot * StackOffsetLocalY;
                child.localPosition = lp;
                slot++;
            }
        }

        private int GetRoomNodeUnderPointer(PointerEventData eventData)
        {
            if (mapSection == null) return -1;
            int i = 0;
            while (true)
            {
                var node = mapSection.Find($"RoomNode_{i}");
                if (node == null) break;
                var rt = node.GetComponent<RectTransform>();
                if (rt != null && RectTransformUtility.RectangleContainsScreenPoint(rt, eventData.position, eventData.pressEventCamera))
                    return i;
                i++;
            }
            return -1;
        }
    }
}
