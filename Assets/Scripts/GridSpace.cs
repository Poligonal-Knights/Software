using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using UnityEngine;

public class GridSpace
{
    GridManager gridManager;
    public Vector3Int gridPosition;
    Entity entity;
    HashSet<Activatable> activatables = new HashSet<Activatable>();

    public Dictionary<string, GridSpace> neighbors = new Dictionary<string, GridSpace>(6);
    //public Dictionary<string, GridSpace> moves = new Dictionary<string, GridSpace>(4);
    public HashSet<GridSpace> moves = new HashSet<GridSpace>();
    public HashSet<GridSpace> jumps = new HashSet<GridSpace>();

    bool passable;
    public bool visited;
    public bool selectable;
    public bool selected;
    public bool affected;

    public BFS_Node node;

    public GridSpace(GridManager gManager, Vector3Int gPosition)
    {
        gridManager = gManager;
        gridPosition = gPosition;
        visited = false;
        passable = false;
    }

    public void SetEntity(Entity e)
    {
        entity = e;
    }

    public Entity GetEntity()
    {
        return entity;
    }

    public bool IsEmpty()
    {
        if (entity is null) return true;
        return false;
    }

    public void ActivateActivatables(PJ Activator)
    {
        foreach(var a in activatables)
        {
            a.Activate(Activator);
        }
    }

    public void AddActivatable(Activatable add)
    {
        activatables.Add(add);
    }

    public void RemoveActivatable(Activatable add)
    {
        activatables.Remove(add);
    }

    public bool HasActivatable()
    {
        return activatables.Any();
    }

    public bool HasTrap()
    {
        if (entity is Trap) return true;
        else return false;
    }

    public bool HasBlock()
    {
        if (entity is Block) return true;
        return false;
    }

    public void GetAdyacentSpaces()
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
        var directions = new[] { "left", "right", "forward", "back" };

        var spaceAtCheckingHeight = this;
        while (spaceAtCheckingHeight is not null && !spaceAtCheckingHeight.HasBlock())
        {
            foreach (var direction in directions)
            {
                var candidateSpace = spaceAtCheckingHeight.neighbors[direction];
                if (candidateSpace.IsPassable())
                {
                    //jumps.Add(candidateSpace);
                    moves.Add(candidateSpace);
                }
            }
            spaceAtCheckingHeight = spaceAtCheckingHeight.neighbors["up"];
        }

        foreach (var direction in directions)
        {
            if (!neighbors[direction].HasBlock())
            {
                spaceAtCheckingHeight = neighbors[direction].neighbors["down"];
                while (spaceAtCheckingHeight is not null && !spaceAtCheckingHeight.IsPassable())
                    spaceAtCheckingHeight = spaceAtCheckingHeight.neighbors["down"];
                if (spaceAtCheckingHeight is not null && spaceAtCheckingHeight.IsPassable())
                    moves.Add(spaceAtCheckingHeight);
            }
        }
    }

    public Vector3 GetPJPlacement()
    {
        //var b = neighbors["down"].GetEntity() as Block;
        //if (b == null)
        //    return GetWorldPosition();
        //return GetWorldPosition() + b.GetPJAdjustment();
        var down = neighbors["down"];
        if (down is not null && down.GetEntity() is Block block)
            return GetWorldPosition() + block.GetPJAdjustment();
        return GetWorldPosition();
    }

    public Vector3 GetWorldPosition()
    {
        return gridPosition + gridManager.getOrigin();
    }

    static public int ManhattanDistance(GridSpace space1, GridSpace space2)
    {
        var vector = space1.gridPosition - space2.gridPosition;
        int distance = Mathf.Abs(vector.x) + Mathf.Abs(vector.y) + Mathf.Abs(vector.z);
        return distance;
    }

    static public int ManhattanDistance2D(GridSpace space1, GridSpace space2)
    {
        var vector = space1.gridPosition - space2.gridPosition;
        int distance = Mathf.Abs(vector.x) + Mathf.Abs(vector.z);
        return distance;
    }

    public int ManhattanDistance(GridSpace otherSpace)
    {
        return ManhattanDistance(this, otherSpace);
    }

    public int ManhattanDistance2D(GridSpace otherSpace)
    {
        return ManhattanDistance2D(this, otherSpace);
    }

    public bool IsPassable()
    {
        return passable;
    }

    public void SetPassable(bool p)
    {
        passable = p;
    }

    public void SetVisited(bool v)
    {
        visited = v;
        if (visited) gridManager.visitedSpaces.Add(this);
        if (neighbors["down"].HasBlock())
        {
            //if (visited && IsEmpty())
            if (visited && (IsEmpty() || HasTrap()))
                (neighbors["down"].GetEntity() as Block).SetInPreviewMode();
            else
                (neighbors["down"].GetEntity() as Block).StopAnimation();
        }
    }

    public bool IsVisited()
    {
        return visited;
    }

    public void SetSelectable(bool s)
    {
        //if (s) gridManager.selectableSpaces.Add(this);
        selectable = s;
        if (neighbors["down"].HasBlock())
        {
            if (selectable)
                (neighbors["down"].GetEntity() as Block).SetInSelectableMode();
            else
                (neighbors["down"].GetEntity() as Block).StopAnimation();
        }
    }

    public bool IsSelectable()
    {
        return selectable;
    }

    public void SetAffected(bool s)
    {
        affected = s;
        //if (affected) gridManager.affectedSpaces.Add(this);
        if (neighbors["down"].HasBlock())
        {
            if (affected)
                (neighbors["down"].GetEntity() as Block).SetInAreaAttackMode();
            else
                (neighbors["down"].GetEntity() as Block).StopAnimation();
        }
    }

    public void SetHealed(bool s)
    {
        affected = s;
        //if (affected) gridManager.affectedSpaces.Add(this);
        if (neighbors["down"].HasBlock())
        {
            if (affected)
                (neighbors["down"].GetEntity() as Block).SetInAreaHealMode();
            else
                (neighbors["down"].GetEntity() as Block).StopAnimation();
        }
    }

    public bool IsAffected()
    {
        return affected;
    }
}
