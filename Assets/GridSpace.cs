using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace
{
    public int x, y, z;
    public Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool hasGround()
    {
        //mirar si el espacio de abajo tiene un bloque walkable
        return false;
    }

    public bool isEmpty()
    {
        return true;
    }

    public void getAdyacentsSpaces()
    {

    }
}
