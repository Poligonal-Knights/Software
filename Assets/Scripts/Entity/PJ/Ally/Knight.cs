using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Ally
{
    bool gritoDeBatalla;

    protected override void Start()
    {
        base.Start();
        //health = 10;
        //pushStrength = 4;
        //trapBonusDamage = 4;
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SetGritoDeBatalla(bool setTo)
    {
        gritoDeBatalla = setTo;
    }

    public bool UsingGritoDeBatalla()
    { 
        return gritoDeBatalla;
    }

    protected override void OnChangeTurn()
    {
        base.OnChangeTurn();
        SetGritoDeBatalla(false);
    }
}
