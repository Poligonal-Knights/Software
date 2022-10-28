using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Ally
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        
    }

    protected override bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        return base.CanMoveThere(start, destination);
    }
}
