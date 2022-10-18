using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS_Node
{
    public GridSpace space;
    public BFS_Node parent;
    public int distance;

    public BFS_Node(GridSpace s, BFS_Node p, int d)
    {
        space = s;
        parent = p;
        distance = d;
        space.node = this;
    }

}
