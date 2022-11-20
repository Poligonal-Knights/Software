using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bomba de aire
public class Wizard_Ability_1 : Ability
{
    public Wizard_Ability_1(PJ owner) : base(owner) { }

    int radius = 5;
    int bombRange = 2;

    public override void Preview()
    {
        var ownerSpacePosition = Owner.GetGridSpace().gridPosition;
        var limits = GridManager.Instance.GetGridSize();
        for (int x = ownerSpacePosition.x - radius; x <= ownerSpacePosition.x + radius; x++)
        {
            for (int z = ownerSpacePosition.z - radius + Mathf.Abs(x - ownerSpacePosition.x); z <= ownerSpacePosition.z + radius - Mathf.Abs(x - ownerSpacePosition.x); z++)
            {
                for (int y = 0; y < limits.y; y++)
                {
                    var candidateSpace = GridManager.Instance.GetGridSpace(x, y, z);
                    if (candidateSpace != null && candidateSpace.IsPassable() && candidateSpace.GetEntity() is not PJ or Trap)
                    {
                        AddSelectableSpace(candidateSpace);
                    }
                }
            }
        }
    }
    public override void SelectTarget(GridSpace selected)
    {
        readyToConfirm = true;
        SelectedSpace = selected;
        ClearAffectedSpaces();
        var bombPosition = SelectedSpace.gridPosition;
        foreach (var space in SelectableSpaces)
            space.SetSelectable(true);
        for (int x = bombPosition.x - bombRange; x <= bombPosition.x + bombRange; x++)
        {
            for (int z = bombPosition.z - bombRange + Mathf.Abs(x - bombPosition.x); z <= bombPosition.z + bombRange - Mathf.Abs(x - bombPosition.x); z++)
            {
                var candidateSpace = GridManager.Instance.GetGridSpace(x, bombPosition.y, z);
                if (candidateSpace != null && candidateSpace.IsPassable())
                {
                    AddAffectedSpace(candidateSpace);
                }
            }
        }
    }

    public override void Confirm()
    {
        Wizard Wizard = Owner as Wizard;
        bool AnyEnemyWasAffected = false;
        Debug.Log(AffectedSpaces.Count);
        foreach (var affectedSpace in AffectedSpaces)
        {
            if (affectedSpace.GetEntity() is Enemy enemy)
            {
                AnyEnemyWasAffected = true;
                var direction = enemy.GetGridSpace().gridPosition - SelectedSpace.gridPosition;
                direction.Set(Math.Sign(direction.x), 0, Math.Sign(direction.z));
                enemy.BePushed(direction, Wizard.pushStrength, Wizard.trapBonusDamage);
            }
        }
        ClearAffectedSpaces();
        ClearSelectableSpaces();
        if (!AnyEnemyWasAffected)
        {
            LogicManager.Instance.PJFinishedMoving();
        }
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
