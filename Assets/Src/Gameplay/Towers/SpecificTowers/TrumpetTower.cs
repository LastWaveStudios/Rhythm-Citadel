using Gameplay.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class TrumpetTower : ATower
    {
        private SpriteRenderer spriteRenderer; //Prevent towers from overlapping incorrectly

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            focusType = FocusStrategies.FirstEnemy;
        }

        void LateUpdate()
        {
            //Lower is, higher appear
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
    }
}

