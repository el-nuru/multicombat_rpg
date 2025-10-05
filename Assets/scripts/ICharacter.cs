namespace c1a_proy.rpg.rpg.Assets.scripts
{
	public interface ICharacter
	{
		string characterName { get; }
		int maxHP { get; }
		int currentHP { get; }
		int speed { get; }
		float FillTime { get; set; }
		float ElapsedTime { get; set; }
		event System.Action<float> OnElapsedTimeChanged;
		void TakeDamage(int amount);
		void Heal(int amount);
		bool IsAlive();
        string FightMessage { get; set; }
        string RunMessage { get; set; }
	}
}
