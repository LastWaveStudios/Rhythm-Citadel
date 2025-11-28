using Gameplay;
using Gameplay.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ObjectPool;

namespace Gameplay.Towers.Bullets
{
    public abstract class ABullet : APoolableObject
    {
        public override void Reset()
        {
            // Not needed, because the shot that will be called instantly after the bullet is get from the pool sets the values
        }
        public void Shot(Vector3 from, List<AEnemy> enemy, float dur, IPoolManager pool, DamageType damageType, int damage)
        {
            Debug.Log("Bullets::Shot: Reached");
            transform.position = from;

            StartCoroutine(BulletMovement(enemy, dur, pool, damageType, damage));
        }
        protected abstract IEnumerator BulletMovement(List<AEnemy> enemyList, float dur, IPoolManager pool, DamageType damageType, int damage);
        protected void DamageEnemies(List<AEnemy> enemyList, DamageType damageType, int damage)
        {
            foreach (AEnemy enemy in enemyList) enemy.TakeDamage(damageType, damage);
        }
    }
}
