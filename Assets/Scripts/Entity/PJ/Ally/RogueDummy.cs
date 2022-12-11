using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

//Esto no tiene futuro, intenta implementar funcionalidades tanto de enemigo como de caballero.
public class RogueDummy : Knight
{
    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnChangeTurn()
    {
        base.OnChangeTurn();
    }

    protected override void UpdateGridSpace()
    {
        if (space != null)
        {
            space.SetEntity(null);
        }
        Vector3Int pos = Vector3Int.RoundToInt(transform.position);
        space = GridManager.Instance.GetGridSpaceWorldCoords(pos);
        space.SetEntity(this);
    }

    protected override void OnMouseUpAsButton()
    {
        //InputHandler.Instance.EntityClicked(this);
        //OnClick.Invoke(this);
        if (!EventSystem.current.IsPointerOverGameObject() && LogicManager.Instance.currentAbility != null)
        {
            LogicManager.Instance.EntityClicked(this);
        }
    }

}
