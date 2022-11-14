using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    protected GridSpace space;

    public UnityEvent<Entity> OnClick;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        TurnManager.Instance.ChangeTurnEvent.AddListener(OnChangeTurn);
    }

    public virtual void Init()
    {
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
        space = GridManager.Instance.GetGridSpaceWorldCoords(pos);
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
        InputHandler.Instance.EntityClicked(this);
        OnClick.Invoke(this);
    }

    protected virtual void OnChangeTurn()
    {

    }

    protected virtual void OnDisable()
    {
        TurnManager.Instance.ChangeTurnEvent.RemoveListener(OnChangeTurn);
    }
}
