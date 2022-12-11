using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

//Bomba de aceite
public class Rogue_Ability_3 : Ability
{
    int attackRange = 4;

    public Rogue_Ability_3(PJ owner) : base(owner) { EnergyConsumed = 0; }

    public override void Preview()
    {
        Debug.Log("Hability Preview");
        var PJSpace = Owner.GetGridSpace();
        var spacesInRange = GridManager.SpacesAtManhattanRange(PJSpace, attackRange);
        foreach (var s in spacesInRange)
            AddSelectableSpace(s);
    }

    public override void SelectTarget(GridSpace selected)
    {
        Debug.Log("Selecting Target");
        ClearAffectedSpaces();
        RefreshSelectableSpaces();
        SelectedSpace = selected;
        Vector3Int init = SelectedSpace.gridPosition - new Vector3Int(1, 0, 1);
        for (int x = 0; x < 3; x++)
            for (int z = 0; z < 3; z++)
            {
                var pos = new Vector3Int(x, 0, z) + init;
                var s = GridManager.Instance.GetGridSpace(pos);
                if (s is not null && s.IsPassable()) AddAffectedSpace(s);
            }
        readyToConfirm = true;
    }

    public override void Confirm()
    {
        base.Confirm();
        Debug.Log("Confirming Hability");
        var rogue = Owner as Rogue;
        var oil = rogue.oil;
        foreach (var affectedSpace in AffectedSpaces)
        {
            Object.Instantiate(oil, affectedSpace.GetWorldPosition(), Quaternion.identity);
        }
        AudioManager.Instance.PlayAttackSound();
        ClearAffectedSpaces();
        ClearSelectableSpaces();
    }

    public override void Cancel()
    {
        base.Cancel();
        ClearAffectedSpaces();
        ClearSelectableSpaces();
    }

    void RefreshSelectableSpaces()
    {
        foreach (var s in SelectableSpaces)
        {
            s.SetSelectable(true);
        }
    }

    public override void ClickedEntity(Entity entityClicked)
    {
        GridSpace spaceToBeSelected = null;
        if (entityClicked is PJ)
        {
            spaceToBeSelected = entityClicked.GetGridSpace();
        }
        else if (entityClicked is Block)
        {
            spaceToBeSelected = entityClicked.GetGridSpace().neighbors["up"];
        }
        if (spaceToBeSelected is not null && spaceToBeSelected.IsSelectable())
            SelectTarget(spaceToBeSelected);
    }
}
