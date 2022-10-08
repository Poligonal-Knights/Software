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

    void Update()
    {
        
    }

    public bool IsWalkable()
    {
        return walkable;
    }
}
