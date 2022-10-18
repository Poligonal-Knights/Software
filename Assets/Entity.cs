using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected GridSpace space;
    protected GameManager gameManager;
    protected GridManager gridManager;

    // Start is called before the first frame update
    public virtual void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gridManager = gameManager.grid;
    }

    public virtual void Init()
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
        space = gm.GetGridSpaceWorldCoords(pos);
        space.SetEntity(this);
    }
}
