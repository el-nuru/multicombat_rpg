using UnityEngine;
using UnityEngine.UI;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>
    /// Muestra un panel con Fight / Run al hacer click en el portrait,
    /// solo si la barra de acción está llena. Actúa sobre el combatant propio.
    /// </summary>
    public class ActionReadyMenu : MonoBehaviour
    {
        private Slider            _actionSlider;
        private GameObject        _panel;
        private Combatant         _combatant;
        private BattleFlowManager _battleFlow;

        private void Awake()
        {
            var sliderGO = transform.Find("SliderAction");
            if (sliderGO != null) _actionSlider = sliderGO.GetComponent<Slider>();

            _panel = BuildPanel();
            _panel.SetActive(false);

            var portraitGO = transform.Find("Portrait");
            if (portraitGO != null)
            {
                var btn = portraitGO.GetComponent<Button>()
                       ?? portraitGO.gameObject.AddComponent<Button>();
                btn.onClick.AddListener(OnPortraitClicked);
            }
        }

        private void Start()
        {
            var autoBind = GetComponent<CharacterUIAutoBind>();
            _combatant  = autoBind?.GetCharacterBehaviour() as Combatant;
            _battleFlow = FindAnyObjectByType<BattleFlowManager>();

            // Conectar botones con la sala correcta de este combatant
            WireActionButton("ARM_FightBtn", () => TriggerAction(PlayerAction.Fight));
            WireActionButton("ARM_RunBtn",   () => TriggerAction(PlayerAction.Run));
        }

        private void TriggerAction(PlayerAction action)
        {
            HideMenu();
            _battleFlow?.TriggerActionForCombatant(_combatant, action);
        }

        private void WireActionButton(string btnName, System.Action callback)
        {
            if (_panel == null) return;
            var t = _panel.transform.Find(btnName);
            if (t == null) return;
            var btn = t.GetComponent<Button>();
            if (btn != null) btn.onClick.AddListener(() => callback());
        }

        private void OnPortraitClicked()
        {
            bool ready = _actionSlider != null && _actionSlider.normalizedValue >= 1f;
            if (ready) _panel.SetActive(!_panel.activeSelf);
        }

        private void HideMenu() => _panel?.SetActive(false);

        // ──────────────────────────────────────────────
        // Builder
        // ──────────────────────────────────────────────
        private GameObject BuildPanel()
        {
            var panel = new GameObject("ActionMenuPanel",
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            panel.transform.SetParent(transform, false);

            var rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta        = new Vector2(76f, 40f);

            var bg = panel.GetComponent<Image>();
            bg.color         = new Color(0.05f, 0.05f, 0.15f, 0.96f);
            bg.raycastTarget = true;

            MakeButton("ARM_FightBtn", "Fight", new Vector2(-20f, 0f))
                .SetParent(panel.transform, false);
            MakeButton("ARM_RunBtn", "Run", new Vector2(20f, 0f))
                .SetParent(panel.transform, false);

            return panel;
        }

        private RectTransform MakeButton(string id, string label, Vector2 pos)
        {
            var go = new GameObject(id,
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));

            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin        = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = pos;
            rt.sizeDelta        = new Vector2(34f, 24f);

            go.GetComponent<Image>().color = new Color(0.18f, 0.34f, 0.54f, 1f);

            var textGO = new GameObject("Text",
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            textGO.transform.SetParent(go.transform, false);

            var textRT       = textGO.GetComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.offsetMin = textRT.offsetMax = Vector2.zero;

            var txt       = textGO.GetComponent<Text>();
            txt.text      = label;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.fontSize  = 10;
            txt.color     = Color.white;
            txt.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            return go.GetComponent<RectTransform>();
        }
    }
}
