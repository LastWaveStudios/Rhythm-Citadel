using UnityEngine;

namespace Gameplay.Enemies
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]

    public class EnemyStats : ScriptableObject
    {
        public int health;
        public int damage;
        public int vinylDrop;
        public int preparationBeats;

        public float reststanceMultiplayer;
    }
}
