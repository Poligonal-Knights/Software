using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Basico
public class Knight_Hability_0 : Hability
{
    public override void Preview()
    {
        Debug.Log("Hability Preview");
        var PJSpace = LogicManager.Instance.GetSelectedPJ().GetGridSpace();
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
        Debug.Log("Selecting Target");
        ClearAffectedSpaces();
        RefreshSelectableSpaces();
        var PJSpace = LogicManager.Instance.GetSelectedPJ().GetGridSpace();
        var auxVector = selected.gridPosition - PJSpace.gridPosition;
        var aux = Vector3.Cross(auxVector, Vector3.up);
        var spaceAffected1 = GridManager.Instance.GetGridSpace(Vector3Int.RoundToInt(selected.gridPosition + aux));
        var spaceAffected2 = GridManager.Instance.GetGridSpace(Vector3Int.RoundToInt(selected.gridPosition - aux));
        AddAffectedSpace(selected);
        AddAffectedSpace(spaceAffected1);
        AddAffectedSpace(spaceAffected2);
        SelectedSpace = selected;
        readyToConfirm = true;
    }

    public override void Confirm()
    {
        Debug.Log("Confirming Hability");
        var knight = LogicManager.Instance.GetSelectedPJ() as Knight;
        var pushDirection = SelectedSpace.gridPosition - knight.GetGridSpace().gridPosition;
        var AnyEnemyWasAffected = false;
        foreach (var affectedSpace in AffectedSpaces)
        {
            var entity = affectedSpace.GetEntity();
            if (entity is Enemy)
            {
                AnyEnemyWasAffected = true;
                var enemy = entity as Enemy;
                enemy.BePushed(pushDirection, knight.pushStrength, knight.trapBonusDamage);
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
