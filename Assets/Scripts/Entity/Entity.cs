using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected GridSpace space;
    protected GameManager gameManager;
    protected GridManager gridManager;
    protected InputHandler inputHandler;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Debug.Log(this+"intentando");
        //gameManager = FindObjectOfType<GameManager>();
        //gridManager = gameManager.gridManager;
        //inputHandler = gameManager.inputHandler;
    }

    public virtual void Init()
    {
        gameManager = FindObjectOfType<GameManager>();
        gridManager = gameManager.gridManager;
        inputHandler = gameManager.inputHandler;
        UpdateGridSpace();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected void UpdateGridSpace()
    {
        if (space != null)
        {
            space.SetEntity(null);
        }
        Vector3Int pos = Vector3Int.RoundToInt(transform.position);
        Debug.Log("ERROR SEGURO INCOMING");
        Debug.Log(gameManager.gridManager);
        space = gameManager.gridManager.GetGridSpaceWorldCoords(pos);
        Debug.Log(space);
        space.SetEntity(this);
    }

    protected void UpdateGridSpace(GridSpace g)
    {
        if (space != null)
        {
            space.SetEntity(null);
        }
        space = g;
        space.SetEntity(this);
    }

    public GridSpace GetGridSpace()
    {
        return space;
    }

    protected virtual void OnMouseUpAsButton()
    {
        inputHandler.EntityClicked(this);
    }
}
