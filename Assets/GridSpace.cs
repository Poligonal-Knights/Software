using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace
{
    GridManager gridManager;
    public Vector3Int gridPosition;
    Entity entity;
    //Trap trap;

    public GridSpace rightSpace, leftSpace, forwardSpace, backSpace, upSpace, downSpace;
    public GridSpace rightMove, leftMove, forwardMove, backMove;// upMove, downMove;

    bool passable;

    public GridSpace(GridManager gManager, Vector3Int gPosition)
    {
        gridManager = gManager;
        gridPosition = gPosition;
        //GetAdyacentsSpaces();
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
        rightSpace = gridManager.GetGridSpace(gridPosition + Vector3Int.right);
        leftSpace = gridManager.GetGridSpace(gridPosition + Vector3Int.left);
        forwardSpace = gridManager.GetGridSpace(gridPosition + Vector3Int.forward);
        backSpace = gridManager.GetGridSpace(gridPosition + Vector3Int.back);
        upSpace = gridManager.GetGridSpace(gridPosition + Vector3Int.up);
        downSpace = gridManager.GetGridSpace(gridPosition + Vector3Int.down);
    }

    public void Link()
    {
        Debug.Log(this.gridPosition);
        if (rightSpace.IsPassable()) rightMove = rightSpace;
        else if (rightSpace.upSpace.IsPassable()) rightMove = rightSpace.upSpace;
        else if (rightSpace.downSpace.IsPassable()) rightMove = rightSpace.downSpace;

        if (leftSpace.IsPassable()) leftMove = leftSpace;
        else if (leftSpace.upSpace.IsPassable()) leftMove = leftSpace.upSpace;
        else if (leftSpace.downSpace.IsPassable()) leftMove = leftSpace.downSpace;

        if (forwardSpace.IsPassable()) forwardMove = forwardSpace;
        else if (forwardSpace.upSpace.IsPassable()) forwardMove = forwardSpace.upSpace;
        else if (forwardSpace.downSpace.IsPassable()) forwardMove = forwardSpace.downSpace;

        if (backSpace.IsPassable()) backMove = backSpace;
        else if (backSpace.upSpace.IsPassable()) backMove = backSpace.upSpace;
        else if (backSpace.downSpace.IsPassable()) backMove = backSpace.downSpace;
    }

    public void SetEntity(Entity e)
    {
        entity = e;
    }
}
