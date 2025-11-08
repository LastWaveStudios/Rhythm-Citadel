using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AGameplayState
{
    [SerializeField] private Tilemap _tilemap;
    public virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InputHandler();
        }
    }

    public virtual Vector3Int GetPositionClicked()
    {
        Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickedPosition.z = 0;

        return _tilemap.WorldToCell(clickedPosition);
    }

    public abstract void InputHandler();
    
}
