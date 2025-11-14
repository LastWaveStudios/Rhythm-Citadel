using Gameplay.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Utilities.ObjectPool;

namespace Gameplay.World
{
    public class Bullet : APoolableObject
    {
        /**
        public bool IsActive
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        /**/
        //Just use IsActive from APoolableObject it's a public method

        /**/
        public override void Reset()
        {
            // Not needed, because the shot that will be called instantly after the bullet is get from the pool sets the values
        }
        /**/

        public void Shot(Vector3 from, List<AEnemy> enemy, float dur, IPoolManager pool, DamageType damageType, int damage)
        {
            Debug.Log("Bullets::Shot: Reached");
            transform.position = from;

            StartCoroutine(BulletMovement(enemy, dur, pool, damageType, damage));
        }

        /// <summary>
        /// Moves the bullet to the enemy position in the duration time, and when reach is stored in pool
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="dur"> must be > 0 </param>
        /// <param name="pool"></param>
        /// <returns></returns>
        private IEnumerator BulletMovement(List<AEnemy> enemyList, float dur, IPoolManager pool, DamageType damageType, int damage)
        {
            Debug.Log("Bullet::BulletMovement: Start the movement of the bullet");
            AEnemy enemy = enemyList[0];
            Vector3 start = transform.position;
            float time = 0f;
            while (time < dur)
            {
                Vector3 position = enemy.transform.position;
                time += Time.deltaTime;
                transform.position = Vector3.Lerp(start, position, time / dur); // It is better to give a dur limited for not to be 0 or negative, but honestly pass that to this is be retard so ...
                yield return null;
            }
            pool.Release(this);
            DamageEnemies(enemyList, damageType, damage);
        }

        private void DamageEnemies(List<AEnemy> enemyList, DamageType damageType, int damage)
        {
            foreach (AEnemy enemy in enemyList) enemy.TakeDamage(damageType, damage);
        }
    }
}
