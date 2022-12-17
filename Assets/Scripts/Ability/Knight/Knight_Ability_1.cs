using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Grito de Batalla
public class Knight_Ability_1 : Ability
{
    public Knight_Ability_1(Knight owner, int energyRequired = 2) : base(owner, energyRequired) { }

    public override void Preview()
    {
        base.Preview();
        Debug.Log("Grito de batalla PREVIEW");
        readyToConfirm = true;
        AddHealedSpace(Owner.GetGridSpace());
    }

    public override void Confirm()
    {
        Debug.Log("Grito de batalla Confirmed");
        var knight = Owner as Knight;
        knight.SetGritoDeBatalla(true);
        LogicManager.Instance.PJFinishedMoving();
        base.Confirm();
    }

    public override void Cancel()
    {
        base.Cancel();
    }
}
