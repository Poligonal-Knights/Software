using System;
using UnityEngine;

//Basico
public class Priest_Ability_0 : Ability
{
    public Priest_Ability_0(Priest owner, int energyRequired = 0) : base(owner, energyRequired, false) { }

    int selectableRadius = 5;

    public override void Preview()
    {
        base.Preview();
        var ownerSpacePosition = Owner.GetGridSpace().gridPosition;
        var limits = GridManager.Instance.GetGridSize();

        for (int x = ownerSpacePosition.x - selectableRadius; x <= ownerSpacePosition.x + selectableRadius; x++)
        {
            for (int z = ownerSpacePosition.z - selectableRadius + Mathf.Abs(x - ownerSpacePosition.x); z <= ownerSpacePosition.z + selectableRadius - Mathf.Abs(x - ownerSpacePosition.x); z++)
            {
                for (int y = 0; y < limits.y; y++)
                {
                    var candidateSpace = GridManager.Instance.GetGridSpace(x, y, z);
                    if (candidateSpace != null && candidateSpace.IsPassable() && candidateSpace.GetEntity() is Enemy)
                    {
                        AddSelectableSpace(candidateSpace);
                    }
                }
            }
        }
    }

    public override void SelectTarget(GridSpace selected)
    {
        ClearAffectedSpaces();
        foreach (var s in SelectableSpaces)
            s.SetSelectable(true);

        readyToConfirm = true;
        SelectedSpace = selected;
        AddAffectedSpace(SelectedSpace);
    }

    public override void Confirm()
    {

        var priest = Owner as Priest;
        foreach (var affectedSpace in AffectedSpaces)
        {
            if (affectedSpace.GetEntity() is Enemy enemy)
            {
                var direction = priest.GetGridSpace().gridPosition - affectedSpace.gridPosition;
                direction.Set(Math.Sign(direction.x), 0, Math.Sign(direction.z));
                enemy.weak = true;
                AudioManager.Instance.PlayAttackSound();
                enemy.BePushed(direction, priest.pushStrength, priest.damage, priest);
            }
        }

        base.Confirm();
    }

    public override void Cancel()
    {
        base.Cancel();
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
