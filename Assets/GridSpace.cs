using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace
{
    Entity entity;

    public GridSpace()
    {

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

    public void GetAdyacentsSpaces()
    {

    }

    public void SetEntity(Entity e)
    {
        entity = e;
    }
}
