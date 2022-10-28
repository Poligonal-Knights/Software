using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;

public class GridSpace
{
    GridManager gridManager;
    public Vector3Int gridPosition;
    Entity entity;
    //Trap trap;

    public Dictionary<string, GridSpace> neighbors = new Dictionary<string, GridSpace>(6);
    public Dictionary<string, GridSpace> moves = new Dictionary<string, GridSpace>(4);

    bool passable;
    public bool visited;

    public BFS_Node node;

    public GridSpace(GridManager gManager, Vector3Int gPosition)
    {
        gridManager = gManager;
        gridPosition = gPosition;
        visited = false;
        passable = false;
    }

    public bool IsEmpty()
    {
        if (entity == null) return true;
        else return false;
    }

    public bool HasBlock()
    {
        if (entity is Block) return true;
        else return false;
    }

    public bool IsPassable()
    {
        return passable;
    }

    public void SetPassable(bool p)
    {
        passable = p;
    }

    public void GetAdyacentsSpaces()
    {
        neighbors["right"] = gridManager.GetGridSpace(gridPosition + Vector3Int.right);
        neighbors["left"] = gridManager.GetGridSpace(gridPosition + Vector3Int.left);
        neighbors["forward"] = gridManager.GetGridSpace(gridPosition + Vector3Int.forward);
        neighbors["back"] = gridManager.GetGridSpace(gridPosition + Vector3Int.back);
        neighbors["up"] = gridManager.GetGridSpace(gridPosition + Vector3Int.up);
        neighbors["down"] = gridManager.GetGridSpace(gridPosition + Vector3Int.down);
    }

    public void Link()
    {
        var directions = new[] { "left", "right", "forward", "back"};

        foreach(var direction in directions)
        {
            if (neighbors[direction].IsPassable()) moves[direction] = neighbors[direction];
            else if (neighbors[direction].neighbors["up"].IsPassable()) moves[direction] = neighbors[direction].neighbors["up"];
            else if (neighbors[direction].neighbors["down"].IsPassable()) moves[direction] = neighbors[direction].neighbors["down"];
        }
    }

    void ChangeVisited()
    {
        visited = !visited;
    }

    public void SetVisited(bool v)
    {
        if(v) gridManager.visitedSpaces.Add(this);
        visited = v;
    }

    public bool IsVisited()
    {
        return visited;
    }

    public void SetEntity(Entity e)
    {
        entity = e;
    }

    public Entity GetEntity()
    {
        return entity;
    }

    public Vector3 GetWorldPosition()
    {
        return gridPosition + gridManager.getOrigin();
    }
}
