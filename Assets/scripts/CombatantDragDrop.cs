using UnityEngine;
using UnityEngine.EventSystems;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class CombatantDragDrop : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private int       combatantIndex;
        [SerializeField] private Transform mapSection;

        private RectTransform _rect;
        private CanvasGroup   _canvasGroup;
        private Vector2       _originalPos;
        private Canvas        _canvas;
        private Combatant     _combatant;
        private AgentMover    _agentMover;

        private void Awake()
        {
            _rect   = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();

            if (!TryGetComponent(out _canvasGroup))
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        private void Start()
        {
            _agentMover = FindAnyObjectByType<AgentMover>();
            _combatant  = SceneQueries.FindAllyCombatantByIndex(combatantIndex);
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
            if (_combatant.RoomIndex == targetRoom) return;

            _agentMover?.MoveTo(_combatant, targetRoom);
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
