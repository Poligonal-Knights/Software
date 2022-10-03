using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject scenary;
    public GameObject NPCS;
    public GridSpace spaces;
    public void Init()
    {
        Recon();
    }

    void Recon()
    {
        int cont = 0;
        foreach (Block e in scenary.GetComponentsInChildren<Block>())
        {
            if(e.isWalkable())
                cont++;
        }
        Debug.Log(cont);
    }
}
