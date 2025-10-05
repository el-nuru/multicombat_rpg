using UnityEngine;
using UnityEngine.UI;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class CharacterUIBinder : MonoBehaviour
    {
        [Header("Sliders")]
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider actionSlider;

        private ICharacter bound;
        private void OnEnable()
        {
            if (bound != null)
                bound.OnElapsedTimeChanged += OnElapsedTimeChanged;
        }
        private void OnDisable()
        {
            if (bound != null)
                bound.OnElapsedTimeChanged -= OnElapsedTimeChanged;
        }
        private void OnElapsedTimeChanged(float newValue)
        {
            if (actionSlider != null)
            {
                float fill = Mathf.Max(0.0001f, bound.FillTime);
                actionSlider.value = Mathf.Clamp01(newValue / fill);
            }
        }

        public void Bind(ICharacter character)
        {
            if (bound != null)
                bound.OnElapsedTimeChanged -= OnElapsedTimeChanged;
            bound = character;
            if (bound != null)
                bound.OnElapsedTimeChanged += OnElapsedTimeChanged;
            InitSliders();
            RefreshAll();
        }

        public void Unbind()
        {
            if (bound != null)
                bound.OnElapsedTimeChanged -= OnElapsedTimeChanged;
            bound = null;
        }

        private void Awake()
        {
            InitSliders();
        }

        private void Update()
        {
            if (bound == null) return;

            // Only update HP value (changes with TakeDamage/Heal)
            if (hpSlider != null)
                hpSlider.value = Mathf.Clamp(bound.currentHP, 0, bound.maxHP);
        }

        private void ConfigureSliders()
        {
            if (hpSlider != null)
            {
                hpSlider.wholeNumbers = true;
                hpSlider.minValue = 0f;
                hpSlider.maxValue = bound != null ? Mathf.Max(1, bound.maxHP) : 1f;
            }
            if (actionSlider != null)
            {
                actionSlider.minValue = 0f;
                actionSlider.maxValue = 1f;
            }
        }

        private void InitSliders()
        {
            ConfigureSliders();
            if (hpSlider != null)
                hpSlider.value = 1f;
            if (actionSlider != null)
                actionSlider.value = 0f;
        }

        public void RefreshAll()
        {
            if (bound == null) return;

            ConfigureSliders();

            if (hpSlider != null)
                hpSlider.value = Mathf.Clamp(bound.currentHP, 0, bound.maxHP);

            if (actionSlider != null)
            {
                float fill = Mathf.Max(0.0001f, bound.FillTime);
                actionSlider.value = Mathf.Clamp01(bound.ElapsedTime / fill);
            }
        }
    }
}
