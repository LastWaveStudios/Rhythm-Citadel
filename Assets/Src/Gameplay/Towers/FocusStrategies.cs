using Gameplay.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Towers
{
    public static class FocusStrategies
    {
        public static List<AEnemy> FirstEnemy(List<AEnemy> enemiesList, Vector3Int startingPos, int range)
        {
            float minDistanceToObjective = Mathf.Infinity;
            AEnemy chosenEnemy = null;
            foreach (AEnemy enemy in enemiesList)
            {
                int distanceToEnemy = GetDistance(startingPos, enemy.GetTile());
                if (IsInRange(distanceToEnemy, range))
                {
                    int distanceToObjective = enemy.GetDistanceToObjective();
                    if (minDistanceToObjective > distanceToObjective) 
                    {
                        minDistanceToObjective = distanceToObjective;
                        chosenEnemy = enemy;
                    }
                }
            }
            return new List<AEnemy> { chosenEnemy };
        }

        public static List<AEnemy> ClosestEnemy(List<AEnemy> enemiesList, Vector3Int startingPos, int range)
        {
            float closestEnemyRange = Mathf.Infinity;
            AEnemy closestEnemy = null;
            foreach (AEnemy enemy in enemiesList)
            {
                int distanceToEnemy = GetDistance(startingPos, enemy.GetTile());
                if (IsInRange(distanceToEnemy, range) && closestEnemyRange > distanceToEnemy)
                {
                    closestEnemy = enemy;
                    closestEnemyRange = distanceToEnemy;
                }
            }

            return new List<AEnemy> { closestEnemy };
        }

        public static List<AEnemy> AreaAttack(List<AEnemy> enemiesList, Vector3Int startingPos, int range)
        {
            List<AEnemy> enemiesInRange = new List<AEnemy>();
            foreach (AEnemy enemy in enemiesList)
            {
                int distanceToEnemy = GetDistance(startingPos, enemy.GetTile());
                if (IsInRange(distanceToEnemy, range))
                {
                    enemiesInRange.Add(enemy);
                }
            }
            return enemiesInRange;
        }

        private static bool IsInRange(int distance, int range)
        {
            return distance <= range;
        }

        private static int GetDistance(Vector3Int pos1, Vector3Int pos2)
        {
            return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
        }
    }
}

