using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Half : Block
{
    protected override void Start()
    {
        base.Start();
    }

    public override Vector3 GetPJAdjustment()
    {
        return new Vector3(.0f, -.5f, -.0f);
    }
}
