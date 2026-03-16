using System.Collections.Generic;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public interface IEnemyStrategy
    {
        ICombatCommand DecideAction(Combatant self, IReadOnlyList<Combatant> targets);
    }
}
