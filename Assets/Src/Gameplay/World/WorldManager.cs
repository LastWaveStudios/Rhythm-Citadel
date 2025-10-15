using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.World
{
    public class WorldManager : Utilities.Singleton<WorldManager>
    {

        [SerializeField] private List<GameObject> _pathObjects;
        [SerializeField] private Tilemap _tilemap;
        private List<Path> _paths;
        private List<TileBase> _tilesWithEnemies;

        //Initialize the Direction array array
        void Start()
        {
            _paths = new List<Path>();
            InitPaths();
        }

        void Update()
        {

        }

        TileBase GetNextTile(int pathID, int currentIndex)
        {
            return _paths[pathID].GetTile(currentIndex + 1);
        }

        void InitPaths()
        {
            foreach (GameObject pathObject in _pathObjects)
            {
                _paths.Add(new Path(pathObject));
            }

        }
        void UpdateEnemiesOnPath(Path path)
        {

        }
    }
}


