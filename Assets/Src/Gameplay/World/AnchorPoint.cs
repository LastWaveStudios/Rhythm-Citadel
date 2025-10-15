using UnityEngine;

namespace Gameplay.World
{
    public enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }
    public class AnchorPoint : MonoBehaviour
    {

        [SerializeField] private int _tilesCount = 0;
        [SerializeField] private Directions _direction = Directions.Up;

        // Cuantos tiles dice cada punto de ancla que agreguemos
        public int getTilesCount()
        {
            return _tilesCount;
        }


        // Devuelve en que dirección indica el tile que hay que moverse
        public Vector3Int GetNextDirection()
        {
            Vector3Int nextDirection = Vector3Int.zero;

            switch (_direction)
            {
                case Directions.Up:
                    nextDirection = Vector3Int.up;
                    break;
                case Directions.Left:
                    nextDirection = Vector3Int.left;
                    break;
                case Directions.Right:
                    nextDirection = Vector3Int.right;
                    break;
                case Directions.Down:
                    nextDirection = Vector3Int.down;
                    break;
                default:
                    Debug.LogError("Una dirección rara: " + _direction);
                    break;
            }

            return nextDirection;
        }
    }
}
