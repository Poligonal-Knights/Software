using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public bool walkable;
    public Material material;
    Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = gameObject.AddComponent<Animator>();
        RuntimeAnimatorController shinyAnim = Resources.Load("ShineAnim") as RuntimeAnimatorController;
        animator.runtimeAnimatorController = shinyAnim;
    }

    public override void Init()
    {
        base.Init();
        //UpdateUpperSpace();
    }

    protected override void Update()
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

    public void SetInPreviewMode()
    {
        animator.SetInteger("animationState", 1);
    }
}
