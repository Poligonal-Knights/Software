using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Shield throw
public class Knight_Ability_4 : Ability
{
    public Knight_Ability_4(PJ owner) : base(owner) { EnergyConsumed = 2; }

    public GameObject shield;

    public override void Preview()
    {
        var PJSpace = Owner.GetGridSpace();
        int y = PJSpace.gridPosition.y;
        for (int x = PJSpace.gridPosition.x - 1; x < PJSpace.gridPosition.x + 2; x++)
            for (int z = PJSpace.gridPosition.z - 1; z < PJSpace.gridPosition.z + 2; z++)
                AddSelectableSpace(GridManager.Instance.GetGridSpace(x, y, z));
        readyToConfirm = true;
    }

    public override void Cancel()
    {
        ClearSelectableSpaces();
        LogicManager.Instance.PJFinishedMoving();
    }

    public override void Confirm()
    {
        base.Confirm();

        foreach (var space in SelectableSpaces)
        {
            if (space.GetEntity() is Ally ally) ally.SetInvencibility(true);
        }
        ClearSelectableSpaces();
    }
}
