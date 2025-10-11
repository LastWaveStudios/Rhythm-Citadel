using System;
using UnityEngine;

public enum Directions
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
public class AnchorPoint : MonoBehaviour
{

    [SerializeField] private int tilesCount = 0;
    [SerializeField] private Directions direction = Directions.UP;

    // Cuantos tiles dice cada punto de ancla que agreguemos
    public int getTilesCount()
    {
        return tilesCount;
    }


    // Devuelve en que dirección indica el tile que hay que moverse
    public Vector3Int GetNextDirection()
    {
        Vector3Int nextDirection = Vector3Int.zero;

        switch (direction)
        {
            case Directions.UP:
                nextDirection = Vector3Int.up;
                break;
            case Directions.LEFT:
                nextDirection = Vector3Int.left;
                break;
            case Directions.RIGHT:
                nextDirection = Vector3Int.right;
                break;
            case Directions.DOWN:
                nextDirection = Vector3Int.down;
                break;
            default:
                Debug.LogError("Una dirección rara: " +  direction);
                break;
        }

        return nextDirection;
    }
}
