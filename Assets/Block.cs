using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public bool walkable;
    public Material material;

    public override void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gridManager = gameManager.grid;
        //meterle componente animator
    }

    public override void Init()
    {
        UpdateGridSpace();
        //UpdateUpperSpace();
    }

    void Update()
    {
        
    }

    public void UpdateUpperSpace()
    {
        if (walkable && !space.neighbors["up"].HasBlock()) space.neighbors["up"].SetPassable(true);
    }

    public bool IsWalkable()
    {
        return walkable;
    }
}
