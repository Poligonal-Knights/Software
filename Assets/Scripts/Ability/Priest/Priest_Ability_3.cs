using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest_Ability_3 : Ability
{
    public Priest_Ability_3(PJ owner) : base(owner) { EnergyConsumed = 2; }

    public override void Preview()
    {
        base.Preview();
        UIManager.Instance.ShowAbilityNonDefined();
    }
    
    public override void Cancel()
    {
        base.Cancel();
        UIManager.Instance.HideAbilityNonDefined();
    }
}
