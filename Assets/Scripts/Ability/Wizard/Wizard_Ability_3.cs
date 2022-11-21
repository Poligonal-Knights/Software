using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Barrido de aire
public class Wizard_Ability_3 : Ability
{
    public Wizard_Ability_3(PJ owner) : base(owner) { EnergyConsumed = 3; }

    int radius = 5;
    int range = 5;

    bool firstSelectionDone;
    HashSet<GridSpace> selectableSpacesForDirection = new HashSet<GridSpace>();
    Vector3Int direction;

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
                    if (candidateSpace != null && candidateSpace.IsPassable())
                    {
                        AddSelectableSpace(candidateSpace);
                    }
                }
            }
        }
    }

    public override void SelectTarget(GridSpace selected)
    {
        Debug.Log("SelTarget");
        firstSelectionDone = true;
        SelectedSpace = selected;
        foreach (var space in SelectableSpaces)
            space.SetSelectable(false);
        ClearAffectedSpaces();
        foreach (var move in SelectedSpace.moves)
            if (move.gridPosition.y == SelectedSpace.gridPosition.y)
            {
                move.SetSelectable(true);
                selectableSpacesForDirection.Add(move);
            }
    }

    public void SelectDirection(GridSpace selected)
    {
        Debug.Log("SelDirection");
        readyToConfirm = true;
        ClearAffectedSpaces();
        foreach (var space in selectableSpacesForDirection)
        {
            space.SetSelectable(true);
        }
        direction = selected.gridPosition - SelectedSpace.gridPosition;
        direction.Set(Math.Sign(direction.x), 0, Math.Sign(direction.z));
        for (int i = 0; i < range; i++)
        {
            var affSpace = GridManager.Instance.GetGridSpace(SelectedSpace.gridPosition + i * direction);
            if (affSpace != null)
                AddAffectedSpace(affSpace);
        }
    }

    public override void Confirm()
    {
        base.Confirm();

        Debug.Log("Confirming Hability");
        var wizard = Owner as Wizard;
        var AnyEnemyWasAffected = false;
        foreach (var affectedSpace in AffectedSpaces)
        {
            if (affectedSpace.GetEntity() is Enemy enemy)
            {
                AnyEnemyWasAffected = true;
                enemy.BePushed(direction, wizard.pushStrength, wizard.trapBonusDamage);
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
        if (firstSelectionDone)
        {
            readyToConfirm = false;
            firstSelectionDone = false;
            ClearAffectedSpaces();
            foreach (var space in selectableSpacesForDirection)
                space.SetSelectable(false);
            selectableSpacesForDirection.Clear();
            foreach (var space in SelectableSpaces)
                space.SetSelectable(true);
        }
        else
        {
            ClearSelectableSpaces();
            base.Cancel();
        }
    }

    public override void ClickedEntity(Entity clickedEntity)
    {
        GridSpace spaceToBeSelected = null;
        if (clickedEntity is PJ)
        {
            spaceToBeSelected = clickedEntity.GetGridSpace();
        }
        else if (clickedEntity is Block)
        {
            spaceToBeSelected = clickedEntity.GetGridSpace().neighbors["up"];
        }
        if (spaceToBeSelected is not null && spaceToBeSelected.IsSelectable())
            if (firstSelectionDone)
                SelectDirection(spaceToBeSelected);
            else SelectTarget(spaceToBeSelected);
    }
}
