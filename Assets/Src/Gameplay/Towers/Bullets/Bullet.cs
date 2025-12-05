using Gameplay.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ObjectPool;

namespace Gameplay.Towers.Bullets
{
    public class Bullet : ABullet
    {



        /// <summary>
        /// Moves the bullet to the enemy position in the duration time, and when reach is stored in pool
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="dur"> must be > 0 </param>
        /// <param name="pool"></param>
        /// <returns></returns>
        protected override IEnumerator BulletMovement(List<AEnemy> enemyList, float dur, IPoolManager pool, DamageType damageType, int damage)
        {

            //Debug.Log("Bullet::BulletMovement: Start the movement of the bullet");

            AEnemy enemy = enemyList[0];
            Vector3 start = transform.position;
            Vector3 end = enemy.transform.position;

            Vector2 dir = end - start;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            float time = 0f;
            while (time < dur)
            {
                time += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, time / dur); // It is better to give a dur limited for not to be 0 or negative, but honestly pass that to this is be retard so ...
                yield return null;
            }
            pool.Release(this);
            DamageEnemies(enemyList, damageType, damage);
        }


    }
}
