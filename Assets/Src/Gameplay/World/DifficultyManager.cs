using Gameplay.Enemies;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Gameplay.World
{
    public class DifficultyManager : Singleton<DifficultyManager>
    {
        public Difficulty currentDifficulty = Difficulty.Normal;

        public Action<Difficulty> OnDifficultyChange;

        public Dictionary<Type, EnemyStats> easyStats;
        public Dictionary<Type, EnemyStats> normalStats;
        public Dictionary<Type, EnemyStats> hardStats;

        // In order for this to work, the stats must be in Resources/Stats/Dificuly !!
        private void Start()
        {
            easyStats = LoadStats("Stats/Easy");
            normalStats = LoadStats("Stats/Normal");
            hardStats = LoadStats("Stats/Hard");
        }
        public EnemyStats GetStats(AEnemy enemy)
        {
            Type t = enemy.GetType();

            try
            {
                return currentDifficulty switch
                {
                    Difficulty.Easy => easyStats[t],
                    Difficulty.Normal => normalStats[t],
                    Difficulty.Hard => hardStats[t],
                    _ => normalStats[t],
                };
            }
            catch (Exception e)
            {
                Debug.Log("Something went wrong trying to get the stats from Difficulty Manager");
                Debug.LogException(e);
            }
            return null;
            
        }

        public void SetDifficulty(Difficulty newDifficulty)
        {
            currentDifficulty = newDifficulty;
            OnDifficultyChange?.Invoke(newDifficulty);
        }

        /// <summary>
        /// Method that loads the stats from the folder so it wont be neccesary to do it manually using the editor
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Dictionary<Type, EnemyStats> LoadStats(string path)
        {
            var tmpDictionary =  new Dictionary<Type, EnemyStats>();

            try
            {
                EnemyStats[] statsArray = Resources.LoadAll<EnemyStats>(path);

                foreach (EnemyStats stats in statsArray)
                {
                    Type t = stats.GetEnemyType();
                    if (t != null) tmpDictionary[t] = stats;
                    else Debug.LogWarning("Stats are null in LoadStats from: " + stats);
                }
                Debug.Log("Happy day, we found some new and fresh stats :)");
                return tmpDictionary;
            }
            catch (Exception e)
            {
                Debug.Log("Something went wrong trying to load the stats");
                Debug.Log(e);
            }
            Debug.Log("Sad day, returning null :( ");
            return null;
            
        }
    }

}
