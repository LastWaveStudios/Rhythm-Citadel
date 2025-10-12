using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 
/// Script que use para ir probando que funcionan las funciones porque no aprendi a usar los test unitarios
/// 
/// </summary>
public class TestingScript : MonoBehaviour
{

    public Tilemap tilemap;
    public GameObject miPath;
    public List<Path> paths;
    private Path script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script = new Path(miPath, tilemap);
        paths.Add(script);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
