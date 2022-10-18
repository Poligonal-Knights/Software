using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameManager gameManager;
    
    GridSpace[,,] spaces;
    Vector3Int minBounds;
    Vector3Int maxBounds;

    public void Init()
    {
        GetBounds();
        //ShowBounds();
        CreateGridSpaces();
        LoadEntities();
        LinkGridSpaces();
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
        var range = maxBounds - minBounds + new Vector3Int(1, 2, 1);
        spaces = new GridSpace[range.x, range.y, range.z];

        for (int i = 0; i < range.x; i++)
            for (int j = 0; j < range.y; j++)
                for (int k = 0; k < range.z; k++)
                    spaces[i, j, k] = new GridSpace(this, new Vector3Int(i, j, k));
    }

    void LoadEntities()
    {
        foreach (var s in spaces)
            s.GetAdyacentsSpaces();

        foreach(var e in FindObjectsOfType<Entity>())
        {
            e.Init();
        }
    }

    void LinkGridSpaces()
    {
        foreach (var space in spaces)
        {
            if (space.IsPassable())
            {
                space.Link();
            }
        }
    }

    public GridSpace GetGridSpace(Vector3Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.z < 0 ||
            coords.x > spaces.GetUpperBound(0) || coords.y > spaces.GetUpperBound(1) || coords.z > spaces.GetUpperBound(2))
            return null;
        return spaces[coords.x, coords.y, coords.z];
    }

    public GridSpace GetGridSpaceWorldCoords(Vector3Int worldCoords)
    {
        var gridCoords = worldCoords - minBounds;
        return spaces[gridCoords.x, gridCoords.y, gridCoords.z];
    }

    public Vector3 getOrigin()
    {
        return minBounds;
    }
}


