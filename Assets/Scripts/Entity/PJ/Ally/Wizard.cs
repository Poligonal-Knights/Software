using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Ally
{

    protected override void Awake()
    {
        base.Awake();
        CanJump = true;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
