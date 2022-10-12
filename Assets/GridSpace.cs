using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace
{
    GridManager gridManager;
    Vector3Int gridPosition;
    Entity entity;
    //Trap trap;

    GridSpace right, left, forward, back, up, down;

    public GridSpace(GridManager grManager, Vector3Int gPosition)
    {
        gridManager = grManager;
        gridPosition = gPosition;
    }

    public bool HasGround()
    {
        //mirar si el espacio de abajo tiene un bloque walkable
        return false;
    }

    public bool IsEmpty()
    {
        if (entity == null) return true;
        else return false;
    }

    public void Link()
    {
        right = gridManager.GetGridSpace(gridPosition + Vector3Int.right);
        left = gridManager.GetGridSpace(gridPosition + Vector3Int.left);
        forward = gridManager.GetGridSpace(gridPosition + Vector3Int.forward);
        back = gridManager.GetGridSpace(gridPosition + Vector3Int.back);
        up = gridManager.GetGridSpace(gridPosition + Vector3Int.up);
        down = gridManager.GetGridSpace(gridPosition + Vector3Int.down);
    }

    public void GetAdyacentsSpaces()
    {

    }

    public void SetEntity(Entity e)
    {
        entity = e;
    }
}
