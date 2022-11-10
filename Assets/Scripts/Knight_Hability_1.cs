using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Knight_Hability_1 : Hability
{
    public override void Preview()
    {
        GridManager.Instance.ClearAffectedSpaces();
        var PJSpace = LogicManager.Instance.GetSelectedPJ().GetGridSpace();
        foreach (var move in PJSpace.moves.Values)
        {
            if (move.gridPosition.y == PJSpace.gridPosition.y)
            {
                move.SetSelectable(true);
                var b = move.neighbors["down"].GetEntity() as Block;
                b.SetInSelectedMode();
            }
        }
        ActivateSelectableAnimation();
    }

    public override void SelectTarget(GridSpace selected)
    {
        Debug.Log("PJ_SelectingDirection");
        var PJSpace = LogicManager.Instance.GetSelectedPJ().GetGridSpace();
        //IsSelectingDirection = false;
        var auxVector = selected.gridPosition - PJSpace.gridPosition;
        var aux = Vector3.Cross(auxVector, Vector3.up);
        var spaceAffected1 = GridManager.Instance.GetGridSpace(Vector3Int.RoundToInt(selected.gridPosition + aux));
        var spaceAffected2 = GridManager.Instance.GetGridSpace(Vector3Int.RoundToInt(selected.gridPosition - aux));
        AffectedSpaces.Add(selected);
        AffectedSpaces.Add(spaceAffected1);
        AffectedSpaces.Add(spaceAffected2);
        ActivateAffectedAnimation();
    }

    public override void Confirm()
    {
        //IsConfirming = false;
        var knight = LogicManager.Instance.GetSelectedPJ() as Knight;
        var PJSpace = knight.GetGridSpace();
        var pushDirection = PJSpace.gridPosition - PJSpace.gridPosition;
        var AnyEnemyWasAffected = false;
        foreach (var affectedSpace in GridManager.Instance.affectedSpaces)
        {
            // Debug.Log(affectedSpace.gridPosition);
            var entity = affectedSpace.GetEntity();
            if (entity is Enemy)
            {
                AnyEnemyWasAffected = true;
                var enemy = entity as Enemy;
                enemy.BePushed(pushDirection, knight.pushStrength, knight.trapBonusDamage);
            }
        }
        if (!AnyEnemyWasAffected)
        {
            LogicManager.Instance.PJFinishedMoving();
        }
    }

    void ActivateSelectableAnimation()
    {
        foreach (var s in SelectableSpaces)
        {
            (s.neighbors["down"].GetEntity() as Block).SetInSelectedMode();
        }
    }

    void ActivateAffectedAnimation()
    {
        foreach (var s in AffectedSpaces)
        {
            (s.neighbors["down"].GetEntity() as Block).SetInAreaAttackMode();
        }
    }
}
