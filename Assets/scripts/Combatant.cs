using UnityEngine;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>
    /// Single character class for all combatants (players, allies, enemies).
    /// Replaces Allied, PlayerCharacter, Enemy, and EnemyCharacter.
    /// Set isEnemy in the Inspector to differentiate enemies from allies.
    /// </summary>
    public class Combatant : MonoBehaviour, ICharacter
    {
        [Header("Identity")]
        [SerializeField] private string _characterName;
        [SerializeField] private bool _isEnemy;
        [SerializeField] private int _roomIndex;

        [Header("Stats")]
        [SerializeField] private int _maxHP = 10;
        [SerializeField] private int _currentHP = 10;
        [SerializeField] private int _speed = 5;
        [SerializeField] private int _strength = 5;
        [SerializeField] private int _defense = 5;
        [SerializeField] private int _power = 5;

        [Header("Action Timer")]
        [SerializeField] private float _fillTime = 3f;

        [Header("Combat Messages")]
        [SerializeField] private string _runMessage = "RUN";

        private float _elapsedTime;

        // ICharacter properties
        public string CharacterName => _characterName;
        public int MaxHP => _maxHP;
        public int CurrentHP => _currentHP;
        public int Speed => _speed;
        public int Strength => _strength;
        public int Defense => _defense;
        public int Power => _power;
        public bool IsEnemy => _isEnemy;
        public int RoomIndex => _roomIndex;
        public string RunMessage => _runMessage;
        public float FillTime
        {
            get => _fillTime;
            set => _fillTime = Mathf.Max(0.01f, value);
        }

        public event System.Action<float> OnElapsedTimeChanged;

        public float ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                if (_elapsedTime != value)
                {
                    _elapsedTime = value;
                    OnElapsedTimeChanged?.Invoke(_elapsedTime);
                }
            }
        }

        public void TakeDamage(int amount)
        {
            _currentHP = Mathf.Max(0, _currentHP - Mathf.Max(0, amount));
        }

        public void Heal(int amount)
        {
            _currentHP = Mathf.Min(_maxHP, _currentHP + Mathf.Max(0, amount));
        }

        public bool IsAlive() => _currentHP > 0;

        private void Update()
        {
            if (IsAlive()) ElapsedTime += Time.deltaTime;
        }
    }
}
