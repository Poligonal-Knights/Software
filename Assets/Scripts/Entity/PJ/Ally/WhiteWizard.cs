using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteWizard : Ally
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 8;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
