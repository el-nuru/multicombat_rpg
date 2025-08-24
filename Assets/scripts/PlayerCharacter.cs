using System.Collections.Generic;
using UnityEngine;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
	public class PlayerCharacter : MonoBehaviour, ICharacter
	{
		[SerializeField] private string _characterName;
		[SerializeField] private int _maxHP;
		[SerializeField] private int _currentHP;
		[SerializeField] private int _speed;
		[SerializeField] private int strength;
		[SerializeField] private int defense;
		[SerializeField] private int power;
		[SerializeField] public List<Skill> skills;
		[SerializeField] private float fillTime;
		[Header("Messages")]
		[SerializeField] private string fightMessage;
		[SerializeField] private string runMessage;
		private float elapsedTime;

		public string characterName { get { return _characterName; } }
		public int maxHP { get { return _maxHP; } }
		public int currentHP { get { return _currentHP; } }
		public int speed { get { return _speed; } }

		public void TakeDamage(int amount)
		{
			_currentHP = Mathf.Max(_currentHP - amount, 0);
		}

		public void Heal(int amount)
		{
			_currentHP = Mathf.Min(_currentHP + amount, _maxHP);
		}

		public bool IsAlive()
		{
			return _currentHP > 0;
		}

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
		public float FillTime { get => fillTime; set => fillTime = value; }
		public bool IsEnemy => false;
		public string FightMessage { get => fightMessage; set => fightMessage = value; }
		public string RunMessage { get => runMessage; set => runMessage = value; }
		public int Strength => strength;
		public int Defense => defense;
		public int Power => power;
		public List<Skill> Skills => skills;

	}
}
