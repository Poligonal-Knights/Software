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
        Debug.Log("Intentando animator");
        base.Start();
        animator = gameObject.AddComponent<Animator>();
        RuntimeAnimatorController shinyAnim = Resources.Load("ShineAnim") as RuntimeAnimatorController;
        animator.runtimeAnimatorController = shinyAnim;
        Debug.Log("Intentando animator 2");
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
        animator.Play("gStoneAnim", -1, 0f);
    }

    public void StopAnimation()
    {
        animator.SetInteger("animationState", 0);
        animator.Play("gStoneNoAnim", -1, 0f);

    }
    public void SetInSelectedMode()
    {
        animator.SetInteger("animationState", 2);
        animator.Play("gStoneAnimSelec", -1, 0f);

    }
    public void SetInAreaAttackMode()
    {
        animator.SetInteger("animationState", 3);
        animator.Play("gStoneAttack", -1, 0f);

    }
}
