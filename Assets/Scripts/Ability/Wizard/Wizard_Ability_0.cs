using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basico
public class Wizard_Ability_0 : Ability
{
    public Wizard_Ability_0(PJ owner) : base(owner) { EnergyConsumed = 0; }

    int range = 3;
    Vector3Int direction;

    public override void Preview()
    {

        var PJSpace = Owner.GetGridSpace();
        foreach (var move in PJSpace.moves)
        {
            if (move.gridPosition.y == PJSpace.gridPosition.y)
            {
                AddSelectableSpace(move);
            }
        }
    }

    public override void SelectTarget(GridSpace selected)
    {
        readyToConfirm = true;
        SelectedSpace = selected;
        ClearAffectedSpaces();
        foreach (var s in SelectableSpaces)
            s.SetSelectable(true);
        direction = SelectedSpace.gridPosition - Owner.GetGridSpace().gridPosition;
        for(int i = 1; i <= range; i++)
        {
            var affSpace = GridManager.Instance.GetGridSpace(SelectedSpace.gridPosition + direction * i);
            AddAffectedSpace(affSpace);
        }
    }

    public override void Confirm()
    {
        base.Confirm();


        var wizard = Owner as Wizard;
        var AnyEnemyWasAffected = false;
        foreach (var affectedSpace in AffectedSpaces)
        {
            if (affectedSpace.GetEntity() is Enemy enemy)
            {
                AnyEnemyWasAffected = true;
                enemy.BePushed(direction, wizard.pushStrength, wizard.trapBonusDamage, wizard);
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
