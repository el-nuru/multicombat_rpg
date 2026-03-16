using System.Collections.Generic;
using UnityEngine;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>
    /// Define las conexiones entre habitaciones y calcula rutas por BFS.
    /// Agregar en el Inspector: cada entrada en Connections define un par de habitaciones adyacentes.
    /// </summary>
    public class RoomGraph : MonoBehaviour
    {
        [System.Serializable]
        public struct RoomConnection
        {
            public int roomA;
            public int roomB;
        }

        [SerializeField] private List<RoomConnection> connections = new();

        private Dictionary<int, List<int>> _adj;

        private void Awake() => BuildAdjacency();

        private void BuildAdjacency()
        {
            _adj = new Dictionary<int, List<int>>();
            foreach (var c in connections)
            {
                if (!_adj.ContainsKey(c.roomA)) _adj[c.roomA] = new List<int>();
                if (!_adj.ContainsKey(c.roomB)) _adj[c.roomB] = new List<int>();
                _adj[c.roomA].Add(c.roomB);
                _adj[c.roomB].Add(c.roomA);
            }
        }

        /// <summary>
        /// Retorna el camino más corto de 'from' a 'to' (sin incluir 'from').
        /// Retorna null si no hay camino.
        /// </summary>
        public List<int> GetPath(int from, int to)
        {
            if (from == to) return new List<int>();
            if (_adj == null) BuildAdjacency();

            var visited = new HashSet<int> { from };
            var queue   = new Queue<List<int>>();
            queue.Enqueue(new List<int> { from });

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                int current = path[path.Count - 1];

                if (!_adj.TryGetValue(current, out var neighbors)) continue;

                foreach (int next in neighbors)
                {
                    if (visited.Contains(next)) continue;
                    var newPath = new List<int>(path) { next };
                    if (next == to)
                    {
                        newPath.RemoveAt(0); // quitar 'from'
                        return newPath;
                    }
                    visited.Add(next);
                    queue.Enqueue(newPath);
                }
            }
            return null;
        }
    }
}
