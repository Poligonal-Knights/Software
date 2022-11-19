using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Ally
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 8;
        pushStrength = 4;
        trapBonusDamage = 4;
        CanJump = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
