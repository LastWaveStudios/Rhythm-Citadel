using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : Utilities.Singleton<WorldManager>
{

    [SerializeField] private List<GameObject> pathObjects;
    [SerializeField] private Tilemap tilemap;
    private List<Path> paths;
    private List<TileBase> tilesWithEnemies;

    //Initialize the Direction array array
    void Start()
    {
        paths = new List<Path>();
        InitPaths();
    }

    void Update()
    {

    }

    TileBase GetNextTile(int pathID, int currentIndex)
    {
        return paths[pathID].GetTile(currentIndex+1);
    }

    void InitPaths()
    {
        foreach (GameObject pathObject in pathObjects)
        {
            paths.Add(new Path(pathObject));
        }

    }
    void UpdateEnemiesOnPath(Path path)
    {

    }
}
