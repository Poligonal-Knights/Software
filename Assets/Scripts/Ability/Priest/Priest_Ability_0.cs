using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

//Basico
public class Priest_Ability_0 : Ability
{
    public Priest_Ability_0(PJ owner) : base(owner) { EnergyConsumed = 0; }

    int selectableRadius = 5;

    public override void Preview()
    {
        Debug.Log("Priest H1 preview");
        var ownerSpacePosition = Owner.GetGridSpace().gridPosition;
        var limits = GridManager.Instance.GetGridSize();

        for (int x = ownerSpacePosition.x - selectableRadius; x <= ownerSpacePosition.x + selectableRadius; x++)
        {
            for (int z = ownerSpacePosition.z - selectableRadius + Mathf.Abs(x - ownerSpacePosition.x); z <= ownerSpacePosition.z + selectableRadius - Mathf.Abs(x - ownerSpacePosition.x); z++)
            {
                for (int y = 0; y < limits.y; y++)
                {
                    var candidateSpace = GridManager.Instance.GetGridSpace(x, y, z);
                    if (candidateSpace != null && candidateSpace.IsPassable() && candidateSpace.GetEntity() is Enemy)
                    {
                        AddSelectableSpace(candidateSpace);
                    }
                }
            }
        }
    }

    public override void SelectTarget(GridSpace selected)
    {
        ClearAffectedSpaces();
        foreach (var s in SelectableSpaces)
            s.SetSelectable(true);

        readyToConfirm = true;
        SelectedSpace = selected;
        AddAffectedSpace(SelectedSpace);
    }

    public override void Confirm()
    {
        base.Confirm();

        var priest = Owner as Priest;
        foreach (var affectedSpace in AffectedSpaces)
        {
            if (affectedSpace.GetEntity() is Enemy enemy)
            {
                var direction = priest.GetGridSpace().gridPosition - affectedSpace.gridPosition;
                direction.Set(Math.Sign(direction.x), 0, Math.Sign(direction.z));
                enemy.BePushed(direction, priest.pushStrength, priest.trapBonusDamage, priest);
            }
        }
        ClearAffectedSpaces();
        ClearSelectableSpaces();
    }

    public override void Cancel()
    {
        base.Cancel();
        ClearAffectedSpaces();
        ClearSelectableSpaces();
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
