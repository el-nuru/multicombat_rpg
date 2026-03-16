namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public interface ICharacter
    {
        string CharacterName { get; }
        int MaxHP { get; }
        int CurrentHP { get; }
        int Speed { get; }
        int Strength { get; }
        int Defense { get; }
        bool IsEnemy { get; }
        float FillTime { get; }
        float ElapsedTime { get; set; }
        string RunMessage { get; }
        event System.Action<float> OnElapsedTimeChanged;
        void TakeDamage(int amount);
        void Heal(int amount);
        bool IsAlive();
    }
}
