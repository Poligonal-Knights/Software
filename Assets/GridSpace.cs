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

    public GridSpace rightSpace, leftSpace, forwardSpace, backSpace, upSpace, downSpace;
    public Dictionary<string, GridSpace> neighbors;// = new Dictionary<string, GridSpace>();
    public Dictionary<string, GridSpace> moves;// = new Dictionary<string, GridSpace>();
    public GridSpace rightMove, leftMove, forwardMove, backMove;// upMove, downMove;

    bool passable;
    public bool visited;

    public GridSpace(GridManager gManager, Vector3Int gPosition)
    {
        gridManager = gManager;
        gridPosition = gPosition;
        visited = false;
    }

    public bool IsEmpty()
    {
        if (entity == null) return true;
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
        if (neighbors["right"].IsPassable()) rightMove = neighbors["right"];
        else if (neighbors["right"].neighbors["up"].IsPassable()) rightMove = neighbors["right"].neighbors["up"];
        else if (neighbors["right"].neighbors["down"].IsPassable()) rightMove = neighbors["right"].neighbors["down"];

        if (neighbors["left"].IsPassable()) leftMove = neighbors["left"];
        else if (neighbors["left"].neighbors["up"].IsPassable()) leftMove = neighbors["left"].neighbors["up"];
        else if (neighbors["left"].neighbors["down"].IsPassable()) leftMove = neighbors["left"].neighbors["down"];

        if (neighbors["forward"].IsPassable()) forwardMove = neighbors["forward"];
        else if (neighbors["forward"].neighbors["up"].IsPassable()) forwardMove = neighbors["forward"].neighbors["up"];
        else if (neighbors["forward"].neighbors["down"].IsPassable()) forwardMove = neighbors["forward"].neighbors["down"];

        if (neighbors["back"].IsPassable()) backMove = neighbors["back"];
        else if (neighbors["back"].neighbors["up"].IsPassable()) backMove = neighbors["back"].neighbors["up"];
        else if (neighbors["back"].neighbors["down"].IsPassable()) backMove = neighbors["back"].neighbors["down"];
    }

    void changeWalked()
    {
        visited = !visited;
    }

    void setWalked(bool w)
    {
        visited = w;
    }

    public void SetEntity(Entity e)
    {
        entity = e;
    }
}
