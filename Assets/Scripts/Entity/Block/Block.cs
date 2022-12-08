using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public bool walkable;
    public Material material;
    public Animator animator;

    protected override void Start()
    {
        base.Start();
    }

    public override void Init()
    {
        base.Init();
        //UpdateUpperSpace();
    }

    protected override void Update()
    {
        
    }

    public void setAnimator(Animator animator)
    {
        this.animator = animator;
    }
    public virtual Animator getAnimator()
    {
        return this.animator;
    }

    public void UpdateUpperSpace()
    {
        if (walkable && !space.neighbors["up"].HasBlock()) space.neighbors["up"].SetPassable(true);
    }

    public bool IsWalkable()
    {
        return walkable;
    }

    public virtual Vector3 GetPJAdjustment()
    {
        return Vector3.zero;
    }

    public virtual void SetInPreviewMode()
    {
        
        getAnimator().SetInteger("animationState", 1);
        getAnimator().Play("gStoneAnim", -1, 0f);
    }

    public virtual void StopAnimation()
    {
        getAnimator().SetInteger("animationState", 0);
        getAnimator().Play("gStoneNoAnim", -1, 0f);
    }
    public virtual void SetInSelectableMode()
    {
        getAnimator().SetInteger("animationState", 2);
        getAnimator().Play("gStoneAnimSelec", -1, 0f);
    }
    public void SetInAreaAttackMode()
    {
        getAnimator().SetInteger("animationState", 3);
        getAnimator().Play("gStoneAttack", -1, 0f);
    }

    public void SetInAreaHealMode()
    {
        getAnimator().SetInteger("animationState", 4);
        getAnimator().Play("gStoneHealAndBuff", -1, 0f);
    }

    protected override void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();
    }
}
