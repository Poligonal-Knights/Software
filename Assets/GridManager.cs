using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject scenary;
    public GameObject NPCS;
    public GridSpace[,,] spaces;

    Vector3Int minBounds;
    Vector3Int maxBounds;

    public void Init()
    {
        GetBounds();
        ShowBounds();
        CreateGridSpaces();
    }

    void GetBounds()
    {
        minBounds = new Vector3Int(Int32.MaxValue, Int32.MaxValue, Int32.MaxValue);
        maxBounds = new Vector3Int(Int32.MinValue, Int32.MinValue, Int32.MinValue);
        Entity[] entities = GameObject.FindObjectsOfType<Entity>();
        foreach (Entity e in entities)
        {
            var pos = Vector3Int.RoundToInt(e.transform.position);
            minBounds = Vector3Int.Min(pos, minBounds);
            maxBounds = Vector3Int.Max(pos, maxBounds);
        }
        minBounds -= Vector3Int.one;
        maxBounds += Vector3Int.one;
    }

    void ShowBounds()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Instantiate(sphere, maxBounds, Quaternion.identity);
        Instantiate(sphere, minBounds, Quaternion.identity);
        sphere.SetActive(false);
    }

    void CreateGridSpaces()
    {
        var range = maxBounds - minBounds;
        spaces = new GridSpace[range.x, range.y, range.z];
        
        for (int i = 0; i < range.x; i++)
            for(int j = 0; j < range.y; j++)
                for(int k = 0; k < range.z; k++)
                    spaces[i, j, k] = new GridSpace();
    }

    public GridSpace GetGridSpaceAt(Vector3Int worldCoords)
    {
        var gridCoords = worldCoords - minBounds;
        return spaces[gridCoords.x, gridCoords.y, gridCoords.z];
    }
}


