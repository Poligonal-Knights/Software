using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : PJ
{
    // Start is called before the first frame update

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
