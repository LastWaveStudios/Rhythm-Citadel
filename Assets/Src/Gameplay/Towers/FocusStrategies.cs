using Gameplay.Enemies;
using Utilities;
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
                if (enemy.isActiveAndEnabled)
                {
                    int distanceToEnemy = Distances.ManhattanDistance(startingPos, enemy.GetTile());
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
            }
            return new List<AEnemy> { chosenEnemy };
        }

        public static List<AEnemy> ClosestEnemy(List<AEnemy> enemiesList, Vector3Int startingPos, int range)
        {
            float closestEnemyRange = Mathf.Infinity;
            AEnemy closestEnemy = null;
            foreach (AEnemy enemy in enemiesList)
            {
                if (enemy.isActiveAndEnabled)
                {
                    int distanceToEnemy = Distances.ManhattanDistance(startingPos, enemy.GetTile());
                    if (IsInRange(distanceToEnemy, range) && closestEnemyRange > distanceToEnemy)
                    {
                        closestEnemy = enemy;
                        closestEnemyRange = distanceToEnemy;
                    }
                }
            }

            return new List<AEnemy> { closestEnemy };
        }

        public static List<AEnemy> AreaAttack(List<AEnemy> enemiesList, Vector3Int startingPos, int range)
        {
            List<AEnemy> enemiesInRange = new List<AEnemy>();
            foreach (AEnemy enemy in enemiesList)
            {
                if (enemy.isActiveAndEnabled)
                {
                    int distanceToEnemy = Distances.ManhattanDistance(startingPos, enemy.GetTile());
                    if (IsInRange(distanceToEnemy, range))
                    {
                        enemiesInRange.Add(enemy);
                    }
                }
            }
            return enemiesInRange;
        }

        private static bool IsInRange(int distance, int range)
        {
            return distance <= range;
        }
    }
}

