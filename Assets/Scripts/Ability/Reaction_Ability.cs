using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction_Ability : Ability
{
    Ally ally;
    Enemy enemy;

    public Reaction_Ability(PJ owner) : base(owner) { }

    public void Engage(Ally ally, Enemy enemy)
    {
        this.ally = ally;
        this.enemy = enemy;
        Preview();
    }

    public override void Preview()
    {
        if(ally.CanReact())
        {
            //ally.SetReactionAvailable(false);
            FreezeEnemies();

        }
    }

    public void FreezeEnemies()
    {
        foreach(var enemy in Object.FindObjectsOfType<Enemy>())
        {
            enemy.SetSpeed(0.0f);
        }
    }

    public void UnFreezeEnemies()
    {
        foreach (var enemy in Object.FindObjectsOfType<Enemy>())
        {
            enemy.ResetSpeed();
        }
    }
}
