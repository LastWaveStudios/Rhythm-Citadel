using System;
using UnityEngine;

namespace Gameplay.Enemies
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]

    public class EnemyStats : ScriptableObject
    {
        [Header("Enemy name")]
        [Tooltip("Nombre COMPLETO (con namespace) de la clase del enemigo. Por ejemplo Gameplay.Enemies.QuarterNote")]
        public string enemyClassName;

        [Header("Stats")]
        public int health;
        public int damage;
        public int vinylDrop;
        public int preparationBeats;

        public float reststanceMultiplayer;
        public Type GetEnemyType()
        {
            return Type.GetType(enemyClassName);
        }
    }
}
