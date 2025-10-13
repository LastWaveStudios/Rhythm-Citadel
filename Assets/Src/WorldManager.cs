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
    private List<Path> pathsWithEnemies;

    //Initialize the Direction array array
    void Start()
    {
        paths = new List<Path>();
        InitPaths();
    }

    void Update()
    {

    }

    void AnchorPoints()
    {

    }
    TileBase GetDirectionFromPath(int pathID, int currentIndex)
    {
        return paths[pathID].GetTileList()[currentIndex];
    }
    void InitPaths()
    {
        int x = 0;
        foreach (GameObject pathObject in pathObjects)
        {
            paths.Add(new Path());
            paths[x].GeneratePath(pathObject, tilemap);
            x++;
        }

        //test

        Debug.Log("Path Getdirection test (1):"+GetDirectionFromPath(0, 0));
        Debug.Log("Path Getdirection test (1+1):"+GetDirectionFromPath(0, 1));
    }
    void UpdateEnemiesOnPath(Path path)
    {

    }
}
