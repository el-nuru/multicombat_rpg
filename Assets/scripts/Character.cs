using UnityEngine;
using System.Collections.Generic;
using c1a_proy.rpg.rpg.Assets.scripts;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public abstract class Character : MonoBehaviour
    {
        [Header("char data")]
        public string characterName;
        public int maxHP;
        public int currentHP;
        public int speed;
        public List<Skill> skills;
        [Header("combat slot")]
        public CombatantSlot slot;

        public virtual void TakeDamage(int amount)
        {
            currentHP = Mathf.Max(currentHP - amount, 0);
        }

        public virtual void Heal(int amount)
        {
            currentHP = Mathf.Min(currentHP + amount, maxHP);
        }

        public bool IsAlive()
        {
            return currentHP > 0;
        }
    }
}
