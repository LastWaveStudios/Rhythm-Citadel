using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.World
{
    public class DifficultyManager : MonoBehaviour
    {
        public static Difficulty currentDifficulty = Difficulty.Normal;

        public EnemyStats easyStats;
        public EnemyStats normaStats;
        public EnemyStats hardStats;

        public static EnemyStats GetStats(Difficulty difficulty, DifficultyManager manager)
        {
            return difficulty switch
            {
                Difficulty.Easy => manager.easyStats,
                Difficulty.Normal => manager.normaStats,
                Difficulty.Hard => manager.hardStats,
                _ => manager.normaStats,
            };
        }

    }

}
