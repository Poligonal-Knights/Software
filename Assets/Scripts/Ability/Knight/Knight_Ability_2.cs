using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Charge
public class Knight_Ability_2 : Ability
{
    public Knight_Ability_2(PJ owner) : base(owner) { EnergyConsumed = 2; }

    /*

    int chargeDistance = 3;
    Vector3Int direction;
    GridSpace finalSpace;

    List<PJ> affectedPJs = new List<PJ>();
    public override void Preview()
    {
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
        SelectedSpace = selected;
        readyToConfirm = true;
        ClearAffectedSpaces();
        RefreshSelectableSpaces();
        var i = 1;
        var obstacleFinded = false;
        direction = SelectedSpace.gridPosition - Owner.GetGridSpace().gridPosition;
        GridSpace affectedSpace = null;
        while (i < chargeDistance && !obstacleFinded)
        {
            affectedSpace = GridManager.Instance.GetGridSpace(SelectedSpace.gridPosition + direction * i);
            if (affectedSpace.IsPassable())
                AddAffectedSpace(affectedSpace);
            else
                obstacleFinded = true;
            if (affectedSpace.GetEntity() is PJ affPJ)
            {
                affectedPJs.Add(affPJ);
            }
            i++;
        }
        finalSpace = affectedSpace;
    }

    public override void Confirm()
    {
        base.Confirm();

        var PJFinalSpace = GridManager.Instance.GetGridSpace(finalSpace.gridPosition - direction * affectedPJs.Count);
        Owner.MoveTo(PJFinalSpace);
        GameManager.Instance.StartCoroutine(MoveAffectedPJs());
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

    void RefreshSelectableSpaces()
    {
        foreach (var s in SelectableSpaces)
        {
            s.SetSelectable(true);
        }
    }

    IEnumerator MoveAffectedPJs()
    {
        for(int i = 0; i < affectedPJs.Count; i++)
        {
            var distanceToKnight = Vector3Int.Distance(affectedPJs[i].GetGridSpace().gridPosition, Owner.GetGridSpace().gridPosition);
            yield return new WaitForSeconds(distanceToKnight * 0.3f);
            affectedPJs[i].MoveTo(GridManager.Instance.GetGridSpace(finalSpace.gridPosition - direction * i));
        }
    }*/
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
