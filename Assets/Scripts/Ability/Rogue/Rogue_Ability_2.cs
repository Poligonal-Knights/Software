using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

//Cerbatana
public class Rogue_Ability_2 : Ability
{
    public Rogue_Ability_2(PJ owner) : base(owner) { EnergyConsumed = 0; }
    int attackRange = 4;

    public override void Preview()
    {
        Debug.Log("Hability Preview");
        Owner = LogicManager.Instance.GetSelectedPJ();
        SelectedSpace = null;
        AffectedSpaces = GridManager.SpacesAtManhattanRange(Owner.GetGridSpace(), attackRange);
        foreach (var s in AffectedSpaces)
        {
            if (s.GetEntity() is Enemy enemy)
            {
                AddSelectableSpace(s);
            }
        }
        foreach (var s in AffectedSpaces)
            s.SetAffected(true);
    }

    public override void SelectTarget(GridSpace selected)
    {
        Debug.Log("Selecting Target");
        //ClearAffectedSpaces();
        RefreshSelectableSpaces();
        if (SelectedSpace != null)
        {
            SelectedSpace.SetAffected(true);
        }
        SelectedSpace = selected;
        SelectedSpace.SetSelectable(true);
        readyToConfirm = true;
    }

    public override void Confirm()
    {
        base.Confirm();
        Debug.Log("Confirming Hability");
        var rogue = LogicManager.Instance.GetSelectedPJ() as Rogue;
        //var pushDirection = SelectedSpace.gridPosition - knight.GetGridSpace().gridPosition;
        var AnyEnemyWasAffected = false;
        foreach (var affectedSpace in AffectedSpaces)
        {
            if (affectedSpace.GetEntity() is Enemy enemy)
            {
                AnyEnemyWasAffected = true;
                //enemy.BePushed(pushDirection, knight.pushStrength, knight.damage, knight);
            }
        }
        ClearAffectedSpaces();
        ClearSelectableSpaces();
            LogicManager.Instance.PJFinishedMoving();
        
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
