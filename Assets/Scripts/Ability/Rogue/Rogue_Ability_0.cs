using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

//Basico
public class Rogue_Ability_0 : Ability
{
    public Rogue_Ability_0(PJ owner) : base(owner) { EnergyConsumed = 0; }

    public override void Preview()
    {
        Debug.Log("Hability Preview");
        var PJSpace = Owner.GetGridSpace();
        foreach (var move in PJSpace.moves)
        {
            if (move.gridPosition.y == PJSpace.gridPosition.y)
            {
                AddSelectableSpace(move);
            }
        }
    }

    public override void SelectTarget(GridSpace selected)
    {
        Debug.Log("Selecting Target");
        ClearAffectedSpaces();
        RefreshSelectableSpaces();
        AddAffectedSpace(selected);
        SelectedSpace = selected;
        readyToConfirm = true;
    }

    public override void Confirm()
    {
        base.Confirm();
        Debug.Log("Confirming Hability");
        var rogue = Owner as Rogue;
        var pushDirection = SelectedSpace.gridPosition - rogue.GetGridSpace().gridPosition;
        var AnyEnemyWasAffected = false;
        GridSpace spawn = null;
        Enemy enemyAffected = null;
        foreach (var affectedSpace in AffectedSpaces)
        {
            if (affectedSpace.GetEntity() is Enemy enemy)
            {
                AnyEnemyWasAffected = true;
                spawn = enemy.GetGridSpace();
                enemyAffected = enemy;
                enemy.BePushed(pushDirection, rogue.pushStrength, rogue.damage, rogue);
            }
        }
        ClearAffectedSpaces();
        ClearSelectableSpaces();
        if (!AnyEnemyWasAffected)
        {
            LogicManager.Instance.PJFinishedMoving();
        }
        else
        {
            rogue.StartCoroutine(spawnCaltrops(rogue.caltrops, spawn, enemyAffected));
        }
    }

    IEnumerator spawnCaltrops(GameObject caltrops, GridSpace spawn, Enemy enemy)
    {
        var canSpawnCaltrops = false;
        var condition = true;
        while (condition)
        {
            yield return new WaitForEndOfFrame();
            if (enemy.health <= 0)
            {
                canSpawnCaltrops = true;
                condition = false;
            }
            else if (enemy.GetGridSpace() != spawn)
            {
                canSpawnCaltrops = true;
                condition = false;
            }
            else if (!enemy.MovementsToDo.Any())
            {
                condition = false;
            }
        }
        if (canSpawnCaltrops)
        {
            var c = Object.Instantiate(caltrops, spawn.GetWorldPosition(), Quaternion.identity);
            c.GetComponent<Caltrops>().Init();
        }
    }

    public override void Cancel()
    {
        base.Cancel();
        ClearAffectedSpaces();
        ClearSelectableSpaces();
    }

    void RefreshSelectableSpaces()
    {
        foreach (var s in SelectableSpaces)
        {
            s.SetSelectable(true);
        }
    }

    public override void ClickedEntity(Entity entityClicked)
    {
        GridSpace spaceToBeSelected = null;
        if (entityClicked is PJ)
        {
            spaceToBeSelected = entityClicked.GetGridSpace();
        }
        else if (entityClicked is Block)
        {
            spaceToBeSelected = entityClicked.GetGridSpace().neighbors["up"];
        }
        if (spaceToBeSelected is not null && spaceToBeSelected.IsSelectable())
            SelectTarget(spaceToBeSelected);
    }
}