using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public bool walkable;
    public Material material;

    void Start()
    {
        //Debug.Log("I'm a block");
    }

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
        if (walkable && space.upSpace.IsEmpty()) space.upSpace.SetPassable(true);
    }

    public bool IsWalkable()
    {
        return walkable;
    }
}
