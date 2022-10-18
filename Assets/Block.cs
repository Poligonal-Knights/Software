using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public bool walkable;
    public Material material;

    public override void Init()
    {
        UpdateGridSpace();
        UpdateUpperSpace();
    }

    void Update()
    {
        
    }

    void UpdateUpperSpace()
    {
        if (walkable && space.neighbors["up"].IsEmpty()) space.neighbors["up"].SetPassable(true);
    }

    public bool IsWalkable()
    {
        return walkable;
    }
}
