using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Block
{
	protected override void Start()
	{
		base.Start();
        animator = gameObject.AddComponent<Animator>();
        RuntimeAnimatorController shinyAnim = Resources.Load("ShineAnim") as RuntimeAnimatorController;
        animator.runtimeAnimatorController = shinyAnim;
    }
}
