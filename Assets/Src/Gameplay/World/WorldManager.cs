using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.World
{
    public class WorldManager : Utilities.Singleton<WorldManager>
    {

        [SerializeField] private List<GameObject> _pathObjects;
        [SerializeField] private Tilemap _tilemap;
        private List<Path> _paths;

        //Initialize the Direction array array
        void Start()
        {
            _paths = new List<Path>();
            InitPaths();
        }

        void Update()
        {

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
    }
}


