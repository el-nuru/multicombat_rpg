using UnityEngine;
using System.Collections;
using c1a_proy.rpg.rpg.Assets.scripts;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class EnemyTurn : MonoBehaviour
    {
        [Header("Current Enemy")]
        public Enemy enemy;
        [Header("Battle Flow Manager")]
        public BattleFlowManager battleFlowManager;

    }
}
