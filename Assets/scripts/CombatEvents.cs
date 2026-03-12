using System;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>
    /// Static event bus for combat. Systems subscribe/unsubscribe without knowing each other.
    /// </summary>
    public static class CombatEvents
    {
        public static event Action<Combatant> OnCombatantDied;

        public static void CombatantDied(Combatant c) => OnCombatantDied?.Invoke(c);
    }
}
