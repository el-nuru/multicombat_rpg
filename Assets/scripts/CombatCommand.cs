namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public interface ICombatCommand
    {
        void Execute();
    }

    public class AttackCommand : ICombatCommand
    {
        private const int MinDamage = 1;

        private readonly Combatant _attacker;
        private readonly Combatant _target;

        public AttackCommand(Combatant attacker, Combatant target)
        {
            _attacker = attacker;
            _target   = target;
        }

        public void Execute()
        {
            int damage = UnityEngine.Mathf.Max(MinDamage, _attacker.Strength - _target.Defense);
            _target.TakeDamage(damage);
            UnityEngine.Debug.Log($"[{_attacker.CharacterName}] attacks [{_target.CharacterName}] for {damage} dmg. HP: {_target.CurrentHP}/{_target.MaxHP}");

            if (!_target.IsAlive())
                CombatEvents.CombatantDied(_target);
        }
    }

    public class RunCommand : ICombatCommand
    {
        private readonly Combatant _actor;
        public RunCommand(Combatant actor) => _actor = actor;

        public void Execute() =>
            UnityEngine.Debug.Log($"[{_actor.CharacterName}] {_actor.RunMessage}");
    }
}
