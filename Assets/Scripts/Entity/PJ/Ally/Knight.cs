using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Ally
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health=10;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override bool CanMoveThere(GridSpace start, GridSpace destination)
    {
        return base.CanMoveThere(start, destination);
    }

    protected override void Hability0()
    {
        foreach(var move in space.moves.Values)
        {
            if(move.gridPosition.y == space.gridPosition.y)
            {
                var b = move.neighbors["down"].GetEntity() as Block;
                b.SetInSelectedMode();
            }
        }
    }
}
