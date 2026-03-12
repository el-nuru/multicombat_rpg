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

        private ICharacter _bound;

        private void OnEnable()  => Subscribe(_bound);
        private void OnDisable() => Unsubscribe(_bound);

        private void Awake() => ConfigureSliders();

        public void Bind(ICharacter character)
        {
            Unsubscribe(_bound);
            _bound = character;
            Subscribe(_bound);
            RefreshAll();
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
