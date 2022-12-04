using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour
{
    protected GridSpace space;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        TurnManager.Instance.ChangeTurnEvent.AddListener(OnChangeTurn);
    }

    public virtual void Init()
    {
        UpdateGridSpace();
    }

    protected virtual void Update()
    {

    }

    protected virtual void UpdateGridSpace()
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

    public void SetGridSpace(GridSpace set)
    {
        space = set;
    }

    protected virtual void OnMouseUpAsButton()
    {
        //InputHandler.Instance.EntityClicked(this);
        //OnClick.Invoke(this);
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            LogicManager.Instance.EntityClicked(this);
        }
    }

    protected virtual void OnChangeTurn()
    {

    }

    protected virtual void OnDisable()
    {
        TurnManager.Instance.ChangeTurnEvent.RemoveListener(OnChangeTurn);
    }
}
