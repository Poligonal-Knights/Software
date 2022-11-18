using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bastion
public class Knight_Hability_3 : Hability
{
    public override void Preview()
    {
        pj = LogicManager.Instance.GetSelectedPJ();
        var PJSpace = pj.GetGridSpace();
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
        foreach (var space in SelectableSpaces)
        {
            if (space.GetEntity() is Ally ally) ally.SetInvencibility(true);
        }
        ClearSelectableSpaces();
    }
}
