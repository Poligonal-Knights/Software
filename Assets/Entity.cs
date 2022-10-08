using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    GridSpace space;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        UpdateGridSpace();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGridSpace()
    {
        GridManager gm = FindObjectOfType<GridManager>();
        Vector3Int pos = Vector3Int.RoundToInt(transform.position);
        space = gm.GetGridSpaceAt(pos);
        space.SetEntity(this);
    }
}
