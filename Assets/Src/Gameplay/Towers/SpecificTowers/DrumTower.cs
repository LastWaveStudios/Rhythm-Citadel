using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class DrumTower : ATower
    {

        private SpriteRenderer spriteRenderer;  //Prevent towers from overlapping incorrectly
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            focusType = FocusStrategies.AreaAttack;
        }

        void LateUpdate()
        {
            //Lower is, higher appear
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
    }
}

