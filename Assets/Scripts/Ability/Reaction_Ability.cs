using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Analytics;
using Object = UnityEngine.Object;

public class Reaction_Ability : Ability
{
    private HashSet<Ally> allies = new HashSet<Ally>();
    public Ally pushedBy;
    Ally ally;
    Enemy enemy;
    private Boolean thisComboFlag = false;
    public Reaction_Ability(PJ owner) : base(owner) { }

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
        if(ally.CanReact() && !allies.Contains(ally))
        {
            ally.SetReactionAvailable(false);
            FreezeEnemies();
            UIManager.Instance.ShowReactionCanvas(true);
            cleanAllies();
        }
    }

    public override void Confirm()
    {
        enemy.comboed += 1;
        var direction = enemy.GetGridSpace().gridPosition - ally.GetGridSpace().gridPosition;
        enemy.MovementsToDo.Clear();//limpiar movimientos enemigos
        enemy.IsMoving = false;
        cleanAllies();
        enemy.BePushed(direction, ally.pushStrength, ally.trapBonusDamage, ally);
        UnFreezeEnemies();
        UIManager.Instance.ShowReactionCanvas(false);
        LogicManager.Instance.PJFinishedMoving();
    }

    public override void Cancel()
    {
        ally.deniedReaction = true;
        UnFreezeEnemies();
        ally.SetReactionAvailable(true);
        UIManager.Instance.ShowReactionCanvas(false);
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
    public void OnChangeTurn(){
        cleanAllies();
    }

    public void cleanAllies()
    {
        pushedBy = null;
        allies.Clear();
    }
}
