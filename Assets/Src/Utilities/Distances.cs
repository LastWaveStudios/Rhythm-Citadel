using UnityEngine;

namespace Utilities
{
    public static class Distances
    {
        public static int ManhattanDistance(Vector3Int pos1, Vector3Int pos2)
        {
            return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
        }
    }
}