using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Rogue : Ally
{
    public GameObject caltrops;
    public GameObject ropeTrap;
    public GameObject oil;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnChangeTurn()
    {
        base.OnChangeTurn();
    }
}
