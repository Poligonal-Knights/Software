using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Knight_Hability_1 : IHability
{
    public void Preview()
    {
        GridManager.Instance.ClearAffectedSpaces();
        foreach (var move in LogicManager.Instance.GetSelectedPJ().GetGridSpace().moves.Values)
        {
            if (move.gridPosition.y == space.gridPosition.y)
            {
                move.SetSelectable(true);
                var b = move.neighbors["down"].GetEntity() as Block;
                b.SetInSelectedMode();
            }
        }
    }

    public void SelectObjective()
    {

    }

    public void Confirm()
    {

    }
}
