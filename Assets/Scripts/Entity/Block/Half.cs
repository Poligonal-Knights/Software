using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Half : Block
{
    protected override void Start()
    {
        base.Start();
        gameObject.transform.Find("Geometry").gameObject.AddComponent<Animator>();
        setAnimator(gameObject.GetComponentInChildren<Animator>());
        RuntimeAnimatorController shinyAnim = Resources.Load("ShineAnim") as RuntimeAnimatorController;
        animator.runtimeAnimatorController = shinyAnim;
    }

    public override Animator getAnimator()
    {
        return gameObject.GetComponentInChildren<Animator>();
    }
    public override Vector3 GetPJAdjustment()
    {
        return new Vector3(.0f, -.5f, -.0f);
    }
    
    // public override void SetInPreviewMode()
    // {
    //     //var trueanimator = gameObject.transform.Find("Geometry"); //.gameObject.GetComponent<Animator>();
    //     var trueanimator = gameObject.GetComponentInChildren<Animator>();
    //     Debug.Log(trueanimator);
    //     trueanimator.SetInteger("animationState", 1);
    //     trueanimator.Play("gStoneAnim", -1, 0f);
    // }
    // public override void StopAnimation()
    // {
    //     var trueanimator = gameObject.GetComponentInChildren<Animator>();
    //     trueanimator.SetInteger("animationState", 0);
    //     trueanimator.Play("gStoneNoAnim", -1, 0f);
    // }
}
