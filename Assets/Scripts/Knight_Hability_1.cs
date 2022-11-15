using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Grito de Batalla
public class Knight_Hability_1 : Hability
{
    public override void Preview()
    {
        Debug.Log("Grito de batalla PREVIEW");
        readyToConfirm = true;
    }

    public override void Confirm()
    {
        Debug.Log("Grito de batalla Confirmed");
        var knight = LogicManager.Instance.GetSelectedPJ() as Knight;
        knight.SetGritoDeBatalla(true);
        LogicManager.Instance.PJFinishedMoving();
    }

    public override void Cancel()
    {
        base.Cancel();
        LogicManager.Instance.PJFinishedMoving();
    }
}
