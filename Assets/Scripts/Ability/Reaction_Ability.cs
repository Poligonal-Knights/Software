using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using Object = UnityEngine.Object;

public class Reaction_Ability : Ability
{
    private HashSet<Ally> allies = new HashSet<Ally>();
    public Ally pushedBy;
    Ally ally;
    Enemy enemy;
    private Boolean thisComboFlag = false;
    public Reaction_Ability() : base() { }
    public Reaction_Ability(Ally owner) : base(owner) { }

    public void Engage(Ally ally, Enemy enemy)
    {
        allies.Add(pushedBy);
        this.ally = ally;
        this.enemy = enemy;
        Preview();
    }

    protected void Update()
    {
        if (thisComboFlag && !enemy.MovementsToDo.Any())
        {
            thisComboFlag = false;
            foreach (var resetAlly in allies)
            {
                resetAlly.deniedReaction = false;
            }
            allies.Clear();
        }
    }
    public override void Preview()
    {
        if(ally.CanReact() && !allies.Contains(ally) && enemy.health >0 )
        {
            enemy.beingPushed = false;
            ally.SetReactionAvailable(false);
            FreezeEnemies();
            UIManager.Instance.ShowReactionCanvas(true);
            allies.Add(ally);
        }
    }
    
    public override void Confirm()
    {
        AudioManager.Instance.Play("AtaqueAliado");
        enemy.comboed += 1;
        var direction = enemy.GetGridSpace().gridPosition - ally.GetGridSpace().gridPosition;
        Debug.Log("Dirección de empuje = " + direction);
        Debug.Log("El enemigo es " + enemy + " y el aliado es "+ally);
        enemy.MovementsToDo.Clear();//limpiar movimientos enemigos
        enemy.StopAllCoroutines();
        enemy.IsMoving = false;
        Debug.Log("Movimientos tras limpiar " + enemy.MovementsToDo.Any());
        cleanAllies();
        enemy.BePushed(direction, ally.pushStrength, ally.damage, ally);
        UnFreezeEnemies();
        UIManager.Instance.ShowReactionCanvas(false);
        //LogicManager.Instance.PJFinishedMoving();
    }

    public override void Cancel()
    {
        enemy.beingPushed = true;
        ally.deniedReaction = true;
        UnFreezeEnemies();
        ally.SetReactionAvailable(true);
        UIManager.Instance.ShowReactionCanvas(false);
    }

    public void FreezeEnemies()
    {
        foreach(var enemy in GameManager.Instance.enemies)
        {
            enemy.SetSpeed(0.0f);
        }
    }

    public void UnFreezeEnemies()
    {
        foreach (var enemy in GameManager.Instance.enemies)
        {
            enemy.ResetSpeed();
        }
    }
    public void OnChangeTurn(){
        cleanAllies();
    }

    public void cleanAllies()
    {
        pushedBy = null;
        allies.Clear();
    }

    public HashSet<Ally> getAllies()
    {
        return allies;
    }
}
