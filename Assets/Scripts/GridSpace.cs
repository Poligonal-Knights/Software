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

    public bool IsEmpty()
    {
        if (entity is null) return true;
        return false;
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
        var directions = new[] { "left", "right", "forward", "back" };

        //moves link
        foreach (var direction in directions)
        {
            if (neighbors[direction].IsPassable()) moves[direction] = neighbors[direction];
            else if (neighbors[direction].neighbors["up"].IsPassable()) moves[direction] = neighbors[direction].neighbors["up"];
            else if (neighbors[direction].neighbors["down"].IsPassable()) moves[direction] = neighbors[direction].neighbors["down"];
        }

        //Jumps link
        //high jumps
        var spaceAtCheckingHeight = neighbors["up"];
        while (spaceAtCheckingHeight is not null && spaceAtCheckingHeight.IsEmpty())
        {
            foreach (var direction in directions)
            {
                var candidateSpace = spaceAtCheckingHeight.neighbors[direction];
                if (candidateSpace.IsPassable())
                {
                    jumps.Add(candidateSpace);
                }
            }
            spaceAtCheckingHeight = spaceAtCheckingHeight.neighbors["up"];
        }
        //low jumps
        foreach(var direction in directions)
        {
            if(neighbors[direction].IsEmpty())
            {
                bool auxCondition = true;
                spaceAtCheckingHeight = neighbors[direction].neighbors["down"];
                while(spaceAtCheckingHeight is not null && !spaceAtCheckingHeight.HasBlock() && auxCondition)
                {
                    if(spaceAtCheckingHeight.IsPassable())
                    {
                        auxCondition = false;
                        jumps.Add(spaceAtCheckingHeight);
                    }
                    spaceAtCheckingHeight = neighbors["down"];
                }
            }
        }
    }

    void ChangeVisited()
    {
        visited = !visited;
    }

    public void SetVisited(bool v)
    {
        visited = v;
        if (visited) gridManager.visitedSpaces.Add(this);
        if (neighbors["down"].HasBlock())
        {
            //if (visited && IsEmpty())
            if(visited && (IsEmpty() || HasTrap()))
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

    public void SetSelected(bool s)
    {
        if (s) gridManager.SetSelectedSpace(this);
        selected = s;
    }

    public bool IsSelected()
    {
        return selected;
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

    public bool IsAffected()
    {
        return affected;
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
