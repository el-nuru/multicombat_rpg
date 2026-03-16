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

        private RectTransform     _rect;
        private CanvasGroup       _canvasGroup;
        private Vector2           _originalPos;
        private Canvas            _canvas;
        private Combatant         _combatant;
        private BattleFlowManager _battleFlow;
        private Transform         _characterUI;
        private Transform[]       _allyUIPerRoom;

        private void Awake()
        {
            _rect   = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();

            if (!TryGetComponent(out _canvasGroup))
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        private void Start()
        {
            _battleFlow = FindAnyObjectByType<BattleFlowManager>();

            _combatant = SceneQueries.FindAllyCombatantByIndex(combatantIndex);

            if (_combatant != null)
                _characterUI = SceneQueries.FindCharacterUI(_combatant)?.transform;

            _allyUIPerRoom = SceneQueries.FindAllyUIPerRoom(_canvas?.transform);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPos               = _rect.anchoredPosition;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha          = 0.7f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_canvas != null)
                _rect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha          = 1f;
            _rect.anchoredPosition      = _originalPos;

            int targetRoom = GetRoomNodeUnderPointer(eventData);
            if (targetRoom < 0 || _combatant == null) return;

            int currentRoom = _combatant.RoomIndex;
            if (currentRoom == targetRoom) return;

            _battleFlow?.MoveToRoom(_combatant, targetRoom);

            if (_characterUI != null && _allyUIPerRoom != null &&
                targetRoom < _allyUIPerRoom.Length && _allyUIPerRoom[targetRoom] != null)
            {
                _allyUIPerRoom[targetRoom].gameObject.SetActive(true);
                _characterUI.SetParent(_allyUIPerRoom[targetRoom], false);
                _characterUI.gameObject.SetActive(true);
            }

            if (_allyUIPerRoom != null && currentRoom < _allyUIPerRoom.Length &&
                _allyUIPerRoom[currentRoom] != null)
            {
                var srcAlly = _allyUIPerRoom[currentRoom];
                srcAlly.gameObject.SetActive(
                    srcAlly.GetComponentInChildren<CharacterUIBinder>(true) != null);
            }

            roomMapUI?.SetCombatantRoom(combatantIndex, targetRoom);
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
                if (rt != null && RectTransformUtility.RectangleContainsScreenPoint(
                        rt, eventData.position, eventData.pressEventCamera))
                    return i;
                i++;
            }
            return -1;
        }
    }
}
