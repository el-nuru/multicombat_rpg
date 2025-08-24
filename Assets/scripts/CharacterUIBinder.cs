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
            bound = null;
        }

        private void Awake()
        {
            InitSliders();
        }

        private void Update()
        {
            if (bound == null) return;

            if (hpSlider != null)
            {
                hpSlider.wholeNumbers = true;
                hpSlider.minValue = 0f;
                hpSlider.maxValue = Mathf.Max(1, bound.maxHP);
                hpSlider.value = Mathf.Clamp(bound.currentHP, 0, bound.maxHP);
            }

            if (actionSlider != null)
            {
                float fill = Mathf.Max(0.0001f, bound.FillTime);
                actionSlider.minValue = 0f;
                actionSlider.maxValue = 1f;
                actionSlider.value = Mathf.Clamp01(bound.ElapsedTime / fill);
            }
        }

        private void InitSliders()
        {
            if (hpSlider != null)
            {
                hpSlider.wholeNumbers = true;
                hpSlider.minValue = 0f;
                hpSlider.maxValue = 1f;
                hpSlider.value = 1f;
            }
            if (actionSlider != null)
            {
                actionSlider.minValue = 0f;
                actionSlider.maxValue = 1f;
                actionSlider.value = 0f;
            }
        }

    public void RefreshAll()
        {
            if (bound == null) return;
            if (hpSlider != null)
            {
                hpSlider.maxValue = Mathf.Max(1, bound.maxHP);
                hpSlider.value = Mathf.Clamp(bound.currentHP, 0, bound.maxHP);
            }
            if (actionSlider != null)
            {
                float fill = Mathf.Max(0.0001f, bound.FillTime);
                actionSlider.value = Mathf.Clamp01(bound.ElapsedTime / fill);
            }
        }
    }
}
