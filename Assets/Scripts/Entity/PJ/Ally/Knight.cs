using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Ally
{
    bool gritoDeBatalla;

    protected override void Start()
    {
        base.Start();
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
        if (TurnManager.Instance.IsPlayerTurn()) //Cambio de ronda
        {
            SetGritoDeBatalla(false);
        }
        //Debug.Log("grito de batalla: " + gritoDeBatalla);

    }

    public override void DealDamage(int damage, PJ attacker = null, PJ damageTo = null)
    {
        if(attacker != null)
        {
            var attackDirection = GetGridSpace().gridPosition - attacker.GetGridSpace().gridPosition;
            var attackDirection2D = new Vector2(attackDirection.x, attackDirection.z);
            var angle = Vector2.Angle(attackDirection2D, -orientation);
            if (angle < 30)
            {
                var distance = GridSpace.ManhattanDistance(GetGridSpace(), attacker.GetGridSpace());
                if (distance > 1) damage = 0;
                else damage -= 1;
            }
        }
        base.DealDamage(damage, attacker);
    }
}
