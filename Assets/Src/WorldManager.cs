using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class WorldManager : Utilities.Singleton<WorldManager>
{
    public Direction[][] paths; 

    //Initialize the Direction array array
    void Start()
    {
        paths = new Direction[0][];
    }

    //Should be called by the level config/level manager whenever a new level is started to clear out the array
    public void NewLevel(int levelPathCount){
        paths = new Direction[levelPathCount][];

        for (int i = 0; i < levelPathCount; i++)
        {
            paths[i] = new Direction[0];
        }
    }

    //Level manager should use this function to add paths, alternatively add path generation here
    public void AddPath(int pathID, Direction[] path) {
        paths[pathID] = path;
    }

    //Returns a whole path as direction array when given a path ID
    public Direction[] GetPath(int pathID)
    {
        return paths[pathID];
    }

    //Returns the NEXT direction of the path to an enemy that calls using its assigned path's ID and their current step
    public Direction GetDirectionFromPath(int pathID, int currentIndex)
    {
        return paths[pathID][currentIndex + 1]; //As I understand, it should return the NEXT direction to the enemy?
    }

    void Update()
    {

        //Test, remove when adding paths properly
        if (Input.GetKeyDown(KeyCode.P))
        {
            NewLevel(2);
            AddPath(0, new Direction[] { Direction.Up, Direction.Right, Direction.Left, Direction.Down });
            AddPath(1, new Direction[] {Direction.Up, Direction.Up, Direction.Down, Direction.Down, Direction.Left,
                                        Direction.Right, Direction.Left, Direction.Right});

            Debug.Log("Path 0:" + string.Join(", ", GetPath(0)));
            Debug.Log("Path 1:" + string.Join(", ", GetPath(1)));
            
            Debug.Log("Path 0 step 1:" + " " + GetDirectionFromPath(0, 0));
            Debug.Log("Path 1 step 5:" + " " + GetDirectionFromPath(1, 4));
        }
        
        

    }
}
