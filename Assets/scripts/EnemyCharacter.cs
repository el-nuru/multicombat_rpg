using System.Collections.Generic;
using UnityEngine;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class EnemyCharacter : MonoBehaviour, ICharacter
    {
        // Core identity and stats
        [SerializeField] private string _characterName;
        [SerializeField] private int _maxHP = 1;
        [SerializeField] private int _currentHP = 1;
        [SerializeField] private int _speed = 1;
        [SerializeField] private int strength;
        [SerializeField] private int defense;
        [SerializeField] private int power;
        [SerializeField] private List<Skill> skills;

        // Turn/Action timer
        [SerializeField] private float fillTime = 2f;
        private float elapsedTime;

        // Messaging
        [SerializeField] private string fightMessage = "ENEMY FIGHT";
        [SerializeField] private string runMessage = "ENEMY RUN";

        // ICharacter implementation
        public string characterName => _characterName;
        public int maxHP => _maxHP;
        public int currentHP => _currentHP;
        public int speed => _speed;
        public float FillTime { get => fillTime; set => fillTime = value; }
        public event System.Action<float> OnElapsedTimeChanged;
        public float ElapsedTime {
            get => elapsedTime;
            set {
                if (elapsedTime != value) {
                    elapsedTime = value;
                    OnElapsedTimeChanged?.Invoke(elapsedTime);
                }
            }
        }
        public string FightMessage { get => fightMessage; set => fightMessage = value; }
        public string RunMessage { get => runMessage; set => runMessage = value; }

        // Optional extras (not part of ICharacter but kept for extensibility)
        public int Strength => strength;
        public int Defense => defense;
        public int Power => power;
        public List<Skill> Skills => skills;
        public bool IsEnemy => true;

        public void TakeDamage(int amount)
        {
            _currentHP = Mathf.Max(0, _currentHP - Mathf.Max(0, amount));
        }

        public void Heal(int amount)
        {
            _currentHP = Mathf.Min(_maxHP, _currentHP + Mathf.Max(0, amount));
        }

        public bool IsAlive()
        {
            return _currentHP > 0;
        }

    }

}