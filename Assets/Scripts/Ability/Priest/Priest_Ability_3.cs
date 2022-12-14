using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest_Ability_3 : Ability
{
    public Priest_Ability_3(Priest owner, int energyRequired = 2) : base(owner, energyRequired) { }

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
