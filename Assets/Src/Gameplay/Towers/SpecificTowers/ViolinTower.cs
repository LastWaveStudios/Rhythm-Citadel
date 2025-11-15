using Gameplay.Towers;
using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class ViolinTower : ATower
    {
        private SpriteRenderer spriteRenderer; //Prevent towers from overlapping incorrectly

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            focusType = FocusStrategies.FirstEnemy;
        }
        
    }
}

