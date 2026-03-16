using System.Collections.Generic;
using UnityEngine;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>Ataca a un aliado aleatorio. Comportamiento por defecto de enemigos.</summary>
    public class AggressiveStrategy : IEnemyStrategy
    {
        public ICombatCommand DecideAction(Combatant self, IReadOnlyList<Combatant> targets)
        {
            if (targets.Count == 0) return null;
            return new AttackCommand(self, targets[Random.Range(0, targets.Count)]);
        }
    }
}
