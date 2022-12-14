using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Swap
public class Priest_Ability_4 : Ability
{
    public Priest_Ability_4(Priest owner, int energyRequired = 2) : base(owner, energyRequired) { }

    GridSpace firstTarget, secondTarget;

    public override void Preview()
    {
        base.Preview();
        foreach (var pj in GameManager.Instance.PJs)
        {
            AddSelectableSpace(pj.GetGridSpace());
        }
    }

    public override void SelectTarget(GridSpace selected)
    {
        foreach (var s in SelectableSpaces)
        {
            s.SetSelectable(true);
        }
        firstTarget = selected;
        firstTarget.SetAffected(true);
    }

    public void SelectSecondTarget(GridSpace selected)
    {
        if(secondTarget != null)
        {
            secondTarget.SetAffected(false);
        }
        foreach (var s in SelectableSpaces)
        {
            s.SetSelectable(true);
        }
        firstTarget.SetAffected(true);
        secondTarget = selected;
        secondTarget.SetAffected(true);
        readyToConfirm = true;
    }

    public override void Confirm()
    {
        var pj1 = firstTarget.GetEntity() as PJ;
        var pj2 = secondTarget.GetEntity() as PJ;
        pj1.gameObject.transform.position = secondTarget.GetWorldPosition();
        pj2.gameObject.transform.position = firstTarget.GetWorldPosition();
        pj1.SetGridSpace(secondTarget);
        pj2.SetGridSpace(firstTarget);
        firstTarget.SetEntity(pj2);
        secondTarget.SetEntity(pj1);

        firstTarget.SetAffected(false);
        secondTarget.SetAffected(false);
        if (pj1 is Enemy enemy) enemy.weak = true;
        if (pj2 is Enemy enemy2) enemy2.weak = true;
        firstTarget?.SetAffected(false);
        firstTarget?.SetSelectable(false);
        secondTarget?.SetAffected(false);
        secondTarget?.SetSelectable(false);
        AudioManager.Instance.Play("Teletransporte");
        LogicManager.Instance.PJFinishedMoving();
        base.Confirm();
    }

    public override void Cancel()
    {
        if (secondTarget is not null)
        {
            secondTarget.SetAffected(false);
            secondTarget.SetSelectable(true);
            secondTarget = null;
            readyToConfirm = false;
        }
        if (firstTarget is not null)
        {
            firstTarget.SetAffected(false);
            firstTarget.SetSelectable(true);
            firstTarget = null;
        }
        else
        {
            firstTarget?.SetAffected(false);
            firstTarget?.SetSelectable(false);
            secondTarget?.SetAffected(false);
            secondTarget?.SetSelectable(false);
            base.Cancel();
        }
    }

    public override void ClickedEntity(Entity clickedEntity)
    {
        GridSpace spaceToBeSelected = null;
        if (clickedEntity is PJ)
        {
            spaceToBeSelected = clickedEntity.GetGridSpace();
        }
        else if (clickedEntity is Block)
        {
            spaceToBeSelected = clickedEntity.GetGridSpace().neighbors["up"];
        }
        if (spaceToBeSelected is not null && spaceToBeSelected.IsSelectable())
            if (firstTarget is not null && firstTarget != spaceToBeSelected)
                SelectSecondTarget(spaceToBeSelected);
            else SelectTarget(spaceToBeSelected);
    }
}
