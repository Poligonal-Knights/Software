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
        gameObject.AddComponent<Animator>();
        Animator currentanimator = gameObject.GetComponent<Animator>();
        RuntimeAnimatorController shinyAnim = Resources.Load("ShineAnim") as RuntimeAnimatorController;
        //Debug.Log(shinyAnim);
        currentanimator.runtimeAnimatorController = shinyAnim;
        currentanimator.SetInteger("estadoAnimacion", 1);
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
