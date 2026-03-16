using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>
    /// Consultas reutilizables sobre la escena. Evita duplicar Find/Sort en múltiples scripts.
    /// </summary>
    public static class SceneQueries
    {
        /// <summary>Devuelve todos los aliados (no-enemy) ordenados por nombre de GameObject.</summary>
        public static List<Combatant> FindAllAllies()
        {
            var all   = Object.FindObjectsByType<Combatant>(FindObjectsInactive.Include);
            var allies = new List<Combatant>();
            foreach (var c in all)
                if (!c.IsEnemy) allies.Add(c);
            allies.Sort((a, b) => string.Compare(a.name, b.name, System.StringComparison.Ordinal));
            return allies;
        }

        /// <summary>Aliado por índice (orden estable por nombre de GameObject).</summary>
        public static Combatant FindAllyCombatantByIndex(int index)
        {
            var allies = FindAllAllies();
            return index >= 0 && index < allies.Count ? allies[index] : null;
        }

        /// <summary>CharacterUIAutoBind cuyo characterBehaviour == combatant.</summary>
        public static CharacterUIAutoBind FindCharacterUI(Combatant combatant)
        {
            if (combatant == null) return null;
            var all = Object.FindObjectsByType<CharacterUIAutoBind>(FindObjectsInactive.Include);
            foreach (var b in all)
                if (b.GetCharacterBehaviour() == combatant) return b;
            return null;
        }

        /// <summary>
        /// Construye el array AllyUI[roomIndex] buscando PanelRoom1, PanelRoom2... en canvasRoot.
        /// </summary>
        public static Transform[] FindAllyUIPerRoom(Transform canvasRoot)
        {
            if (canvasRoot == null) return System.Array.Empty<Transform>();
            var list = new List<Transform>();
            int i = 1;
            while (true)
            {
                var panel = canvasRoot.Find($"PanelRoom{i}");
                if (panel == null) break;
                list.Add(panel.Find("AllyUI"));
                i++;
            }
            return list.ToArray();
        }
    }
}
