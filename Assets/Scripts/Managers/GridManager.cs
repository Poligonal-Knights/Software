using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    GridSpace[,,] spaces;
    Vector3Int minBounds;
    Vector3Int maxBounds;

    public List<GridSpace> visitedSpaces = new List<GridSpace>();
    public HashSet<GridSpace> selectableSpaces = new HashSet<GridSpace>();
    public HashSet<GridSpace> affectedSpaces = new HashSet<GridSpace>();

    GridSpace selectedSpace;

    private void Awake() => Instance = this;

    public void Start()
    {
        
    }

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
        foreach (var e in FindObjectsOfType<Entity>())
        {
            e.Init();
        }
        foreach (var e in FindObjectsOfType<Block>())
        {
            e.UpdateUpperSpace();
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

    public GridSpace GetGridSpace(int x, int y, int z)
    {
        if (x < 0 || y < 0 || z < 0 ||
            x > spaces.GetUpperBound(0) || y > spaces.GetUpperBound(1) || z > spaces.GetUpperBound(2))
            return null;
        return spaces[x, y, z];
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

    public void StopPJMovementPreview()
    {
        //foreach(var space in visitedSpaces)
        //{
        //    var b = space.neighbors["down"].GetEntity() as Block;
        //    b.StopAnimation();
        //}
        ClearVisitedSpaces();
    }

    public void StopPJHabilityPreview()
    {
        //ClearAffectedSpaces();
        foreach(var s in affectedSpaces)
        {
            if (s.neighbors["down"].HasBlock()) (s.neighbors["down"].GetEntity() as Block).StopAnimation();
        }
        ClearSelectableSpaces();
    }

    public void SetSelectedSpace(GridSpace g)
    {
        if(selectedSpace != null)
        {
            selectedSpace.SetSelected(false);
        }
        selectedSpace = g;
    }

    public GridSpace GetSelectedSpace()
    {
        return selectedSpace;
    }

    public void ClearVisitedSpaces()
    {
        foreach (var space in visitedSpaces)
        {
            space.SetVisited(false);
        }
        visitedSpaces.Clear();
    }

    public void ClearSelectableSpaces()
    {
        foreach (var space in selectableSpaces)
        {
            space.SetSelectable(false);
            (space.neighbors["down"].GetEntity() as Block).StopAnimation();
        }
        selectableSpaces.Clear();
    }

    public void ClearAffectedSpaces()
    {
        foreach (var space in affectedSpaces)
        {
            space.SetAffected(false);
            //(space.neighbors["down"].GetEntity() as Block).StopAnimation();
        }
        affectedSpaces.Clear();
    }
}


