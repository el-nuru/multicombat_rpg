using UnityEngine;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class Combatant : MonoBehaviour, ICharacter
    {
        [SerializeField] private int    _roomIndex;
        [SerializeField] private string _characterName = "";
        [SerializeField] private bool   _isEnemy       = false;
        [SerializeField] private int    _maxHP         = 10;
        [SerializeField] private int    _speed         = 5;
        [SerializeField] private int    _strength      = 5;
        [SerializeField] private int    _defense       = 5;
        [SerializeField] private string _runMessage    = "RUN";

        private const float FillTimeBase = 10f;

        private int            _currentHP;
        private float          _elapsedTime;
        private IEnemyStrategy _strategy = new AggressiveStrategy();

        // ── ICharacter ──────────────────────────────────────────────────────────
        public string CharacterName => _characterName;
        public int    MaxHP         => _maxHP;
        public int    Speed         => _speed;
        public int    Strength      => _strength;
        public int    Defense       => _defense;
        public bool   IsEnemy       => _isEnemy;
        public string RunMessage    => _runMessage;
        public int    RoomIndex     => _roomIndex;

        public float FillTime => Mathf.Max(0.5f, FillTimeBase / Mathf.Max(1, Speed));

        public IEnemyStrategy Strategy
        {
            get => _strategy;
            set => _strategy = value ?? new AggressiveStrategy();
        }

        public event System.Action<float> OnElapsedTimeChanged;

        public int CurrentHP => _currentHP;

        public float ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                if (_elapsedTime == value) return;
                _elapsedTime = value;
                OnElapsedTimeChanged?.Invoke(_elapsedTime);
            }
        }

        private void Awake() => _currentHP = MaxHP;

        private void Update()
        {
            if (IsAlive()) ElapsedTime += Time.deltaTime;
        }

        public void TakeDamage(int amount) =>
            _currentHP = Mathf.Max(0, _currentHP - Mathf.Max(0, amount));

        public void Heal(int amount) =>
            _currentHP = Mathf.Min(MaxHP, _currentHP + Mathf.Max(0, amount));

        public bool IsAlive()          => _currentHP > 0;
        public void SetRoomIndex(int i) => _roomIndex = i;
    }
}
