using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bastion
public class Knight_Ability_3 : Ability
{
    public Knight_Ability_3(Knight owner, int energyRequired = 3) : base(owner, energyRequired) { }

    public override void Preview()
    {
        base.Preview();
        var PJSpace = Owner.GetGridSpace();
        int y = PJSpace.gridPosition.y;
        for (int x = PJSpace.gridPosition.x - 1; x < PJSpace.gridPosition.x + 2; x++)
            for (int z = PJSpace.gridPosition.z - 1; z < PJSpace.gridPosition.z + 2; z++)
                AddHealedSpace(GridManager.Instance.GetGridSpace(x, y, z));
        readyToConfirm = true;
    }

    public override void Cancel()
    {
        base.Cancel();
        LogicManager.Instance.PJFinishedMoving();
    }

    public override void Confirm()
    {

        // if (Owner is Ally)
        // {
        //     var owner = Owner as Ally;
        //     owner.SetInvencibility(true);
        // }

        foreach (var space in AffectedSpaces)
        {
            if (space.GetEntity() is Ally ally)
            {
                ally.SetInvencibility(true);
            }
        }
        base.Confirm();
    }
}
