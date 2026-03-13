using UnityEngine;
using UnityEngine.UI;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class CharacterUIBinder : MonoBehaviour
    {
        private const float MinFillTime = 0.0001f;

        [Header("Sliders")]
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider actionSlider;

        [Header("Combatant indicator")]
        [SerializeField] private Color combatantColor = Color.white;

        private ICharacter _bound;
        private Image _colorIndicator;

        private void OnEnable()  => Subscribe(_bound);
        private void OnDisable() => Unsubscribe(_bound);

        private void Awake() { ConfigureSliders(); EnsureIndicator(); }

        public void Bind(ICharacter character)
        {
            Unsubscribe(_bound);
            _bound = character;
            Subscribe(_bound);
            RefreshAll();
        }

        public void SetColor(Color color)
        {
            combatantColor = color;
            EnsureIndicator();
        }

        private void EnsureIndicator()
        {
            if (actionSlider == null) return;
            if (_colorIndicator == null)
            {
                var go = new GameObject("CombatantColor", typeof(RectTransform), typeof(Image));
                go.transform.SetParent(actionSlider.transform, false);
                var rt = go.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0, 0.5f);
                rt.anchorMax = new Vector2(0, 0.5f);
                rt.pivot     = new Vector2(1, 0.5f);
                rt.anchoredPosition = Vector2.zero;
                var h = actionSlider.GetComponent<RectTransform>().rect.height;
                rt.sizeDelta = new Vector2(h, h);
                _colorIndicator = go.GetComponent<Image>();
                _colorIndicator.raycastTarget = false;
            }
            _colorIndicator.color = combatantColor;
        }

        private void Subscribe(ICharacter c)   { if (c != null) c.OnElapsedTimeChanged += OnElapsedTimeChanged; }
        private void Unsubscribe(ICharacter c) { if (c != null) c.OnElapsedTimeChanged -= OnElapsedTimeChanged; }

        public void RefreshAll()
        {
            if (_bound == null) return;
            ConfigureSliders();
            UpdateHP();
            UpdateActionBar(_bound.ElapsedTime);
        }

        private void OnElapsedTimeChanged(float newValue) => UpdateActionBar(newValue);

        private void UpdateHP()
        {
            if (hpSlider != null)
                hpSlider.value = Mathf.Clamp(_bound.CurrentHP, 0, _bound.MaxHP);
        }

        private void UpdateActionBar(float elapsed)
        {
            if (actionSlider != null)
                actionSlider.value = Mathf.Clamp01(elapsed / Mathf.Max(MinFillTime, _bound.FillTime));
        }

        private void ConfigureSliders()
        {
            if (hpSlider != null)
            {
                hpSlider.wholeNumbers = true;
                hpSlider.minValue = 0f;
                hpSlider.maxValue = _bound != null ? Mathf.Max(1, _bound.MaxHP) : 1f;
            }
            if (actionSlider != null)
            {
                actionSlider.minValue = 0f;
                actionSlider.maxValue = 1f;
            }
        }
    }
}
