using Gameplay.Enemies;
using Gameplay.RhythmSystem;
using Gameplay.World;
using System.Collections;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.Enemies
{
    public class QuarterNote : AEnemy
    {
        void Start()
        {
            RhythmManager.Instance.onQuarter += OnRhythmUpdate;
        }

        protected override void OnRhythmUpdate()
        {
            StartCoroutine(MoveToNextTile(_moveTime));
        }

        private IEnumerator MoveToNextTile(double moveTime)
        {
            Vector3Int nextTile = WorldManager.Instance.GetNextTile(_path, _index);
            _index++;

            Vector3Int finalTile = new Vector3Int(0, 0, 1);
            if (nextTile == finalTile)
            {
                Destroy(gameObject);
                yield break;
            }

            Vector3 originPos = transform.position;
            Vector3 targetPos = WorldManager.Instance.GetCellCenterWorld(nextTile);
            float t = 0.0f;
            while (t <= _moveTime)
            {
                //transform.position = Vector3.Lerp(originPos, targetPos, EaseInBack(t / _moveTime));
                float T = EaseInBack(t / _moveTime);
                transform.position = originPos * (1 - T) + targetPos * T;
                t += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;   // Fix for center final positions
        }

        // Taken from https://easings.net/#easeInBack 
        private float EaseInBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1.0f;

            return c3 * t * t * t - c1 * t * t;
        }

        private void OnDestroy()
        {
            RhythmManager.Instance.onQuarter -= OnRhythmUpdate;
        }
    } 
}

