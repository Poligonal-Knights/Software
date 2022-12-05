using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : Entity
{
    bool active;
    protected HashSet<GridSpace> activatableSpaces = new HashSet<GridSpace>();

    public override void Init()
    {
        base.Init();
        SetActivatableSpaces();
    }

    public virtual void Activate(PJ Activator)
    {
        active = true;
    }

    public virtual void Deactivate()
    {
        active = false;
    }

    public bool IsActivated()
    {
        return active;
    }

    protected void AddActivatableSpace(GridSpace spaceToAdd)
    {
        activatableSpaces.Add(spaceToAdd);
        space.AddActivatable(this);
    }

    protected virtual void SetActivatableSpaces()
    {

    }

    protected virtual void RemoveActivatableSpaces()
    {
        foreach(GridSpace s in activatableSpaces)
        {
            s.RemoveActivatable(this);
        }
        activatableSpaces.Clear();
    }

    protected override void UpdateGridSpace()
    {
        //if (space != null)
        //{
        //    space.SetEntity(null);
        //}
        Vector3Int pos = Vector3Int.RoundToInt(transform.position);
        space = GridManager.Instance.GetGridSpaceWorldCoords(pos);
        //space.SetEntity(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        RemoveActivatableSpaces();
    }
}
