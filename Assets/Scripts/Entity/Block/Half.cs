using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Half : Block
{
    protected override void Start()
    {
        base.Start();
        //animator = gameObject.AddComponent<Animator>();
        animator = gameObject.transform.Find("Geometry").gameObject.AddComponent<Animator>();
        RuntimeAnimatorController shinyAnim = Resources.Load("ShineAnim") as RuntimeAnimatorController;
        animator.runtimeAnimatorController = shinyAnim;
    }
}
