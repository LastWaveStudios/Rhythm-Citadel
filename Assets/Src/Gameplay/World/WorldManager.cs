using System;
using System.Collections.Generic;
using System.Text;
using Gameplay.Enemies;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.World
{
    public class WorldManager : Utilities.ServiceLocator.AService
    {

        [SerializeField] private List<GameObject> _pathObjects;
        [SerializeField] private Tilemap _tilemap;
        private List<Path> _paths;

        public override void Init()
        {
            _paths = new List<Path>();
            InitPaths();
        }

        public Vector3Int GetNextTile(int pathID, int currentIndex)
        {
            return _paths[pathID].GetTile(currentIndex + 1);
        }

        public Vector3Int GetTile(int pathID, int index)
        {
            return _paths[pathID].GetTile(index);
        }

        public Vector3 GetCellCenterWorld(Vector3Int CellCoordinates)
        {
            return _tilemap.GetCellCenterWorld(CellCoordinates);
        }

        public Vector3Int GetCellFromWorldPos(Vector3 Pos)
        {
            return _tilemap.WorldToCell(Pos);
        }

        void InitPaths()
        {
            foreach (GameObject pathObject in _pathObjects)
            {
                _paths.Add(new Path(pathObject));
            }

        }

        public List<Vector3Int> GetSpawnPoints()
        {
            List<Vector3Int> spawnPointsList = new List<Vector3Int>();
            foreach (Path pathObject in _paths)
            {
                spawnPointsList.Add(pathObject.GetSpawnPoint());
            }
            return spawnPointsList;
        }

        public int GetTileCount(int pathID)
        {
            return _paths[pathID].GetTileCount();
        }


        public Vector3Int GetLastTile(int pathID)
        {
            return _paths[pathID].GetTile(_paths[pathID].TilesCount - 1);
        }

        public Vector3 GetTileSize()
        {
            return _tilemap.layoutGrid.cellSize;
        }

        public List<Vector3Int> GetTilesInRange(Vector3Int center, int range)
        {
            List<Vector3Int> tilesInRange = new List<Vector3Int>();

            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(y) <= range)
                    {
                        Vector3Int selectedTile = new Vector3Int(center.x + x, center.y + y, center.z);
                        tilesInRange.Add(selectedTile);
                    }

                }
            }
            return tilesInRange;
        }

        public void Highlight(Vector3Int tile, Color color)
        {
            _tilemap.SetTileFlags(tile, TileFlags.None);

            _tilemap.SetColor(tile, color);
        }

        public void ClearHightlight(Vector3Int tile)
        {
            _tilemap.SetColor(tile, Color.white);
        }
    }
}


