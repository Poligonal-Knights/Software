using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Grito de Batalla
public class Knight_Ability_1 : Ability
{
    public Knight_Ability_1(PJ owner) : base(owner) { EnergyConsumed = 1; }

    public override void Preview()
    {
        Debug.Log("Grito de batalla PREVIEW");
        readyToConfirm = true;
        AddHealedSpace(Owner.GetGridSpace());
    }

    public override void Confirm()
    {
        base.Confirm();

        Debug.Log("Grito de batalla Confirmed");
        var knight = LogicManager.Instance.GetSelectedPJ() as Knight;
        knight.SetGritoDeBatalla(true);
        LogicManager.Instance.PJFinishedMoving();
        ClearAffectedSpaces();
        ClearSelectableSpaces();
    }

    public override void Cancel()
    {
        base.Cancel();
        ClearAffectedSpaces();
        ClearSelectableSpaces();
    }
}
