using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Ally
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected override bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        if (start.gridPosition.y == destination.gridPosition.y) return true;
        return false;
    }
}
