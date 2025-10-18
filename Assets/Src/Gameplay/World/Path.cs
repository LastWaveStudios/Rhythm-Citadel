using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.World
{
    /// <summary>
    /// 
    /// Path es una clase y GeneratePath es su constructor
    /// a lo mejor hay que cambiar que en vez de tile base sea un vector3Int que indica la posicion del tile que nos interese.
    /// La linea que creo que funciona como queremos esta comentada pero habria que cambiar el tipo de dato.
    /// Tambien creo que los tiles se agregan como objetos vacios por alguna razon, pero no lo tengo claro
    /// 
    /// 
    /// </summary>
    public class Path
    {
        private List<TileBase> _tileList = new List<TileBase>();

        public Path(GameObject pathToLook) // por ahora esta como par�metro, si usamos el singelton tendr� que cogerlo directamente
        {
            Tilemap tilemap = WorldManager.FindAnyObjectByType<Tilemap>();
            foreach (Transform child in pathToLook.transform)
            {
                GameObject go = child.gameObject;
                Vector3 anchorPos = go.transform.position;

                Vector3Int startingCell = tilemap.WorldToCell(anchorPos);   // La celda en la que esta el ancla

                AnchorPoint anchor = go.GetComponent<AnchorPoint>();    // Todas las anclas tendran que tener ese script
                Vector3Int movementDirection = anchor.GetNextDirection();   // En que direccion nos manda el ancla
                int maxTile = anchor.getTilesCount();   // Lo recojo aqui para no estar todo el rato entrando en el script del AnchorPoint

                for (int i = 0; i < maxTile; i++)
                {
                    _tileList.Add(tilemap.GetTile(startingCell + movementDirection * i));
                    //tileList.Add(startingCell + movementDirection * i);
                   // Debug.Log(startingCell + movementDirection * i);
                }
            }
        }

        public List<TileBase> GetTileList()
        {
            return _tileList;
        }

        public TileBase GetTile(int index)
        {
            //Debug.Log("Longitud de la lista de tiles" + _tileList.Count);
            if (index < _tileList.Count - 1)
            {
                Debug.Log("El index actual es de " + index);
                return _tileList[index];
            }
            return null;
        }
    }
}


