using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basico
public class Wizard_Ability_0 : Ability
{
    public Wizard_Ability_0(Wizard owner, int energyRequired = 2) : base(owner, energyRequired, false) { }

    int range = 3;
    Vector3Int direction;

    public override void Preview()
    {
        base.Preview();
        var PJSpace = Owner.GetGridSpace();
        foreach (var move in PJSpace.moves)
        {
            if (move.gridPosition.y == PJSpace.gridPosition.y)
            {
                AddSelectableSpace(move);
            }
        }
    }
    //MIRAR SI QUIERO QUE PUEDA PEGAR A MELEE (SE CAMBIA EL INT I = 1 POR = 0 Y EL <= RANGE POR <RANGE)
    public override void SelectTarget(GridSpace selected)
    {
        readyToConfirm = true;
        SelectedSpace = selected;
        ClearAffectedSpaces();
        foreach (var s in SelectableSpaces)
            s.SetSelectable(true);
        direction = SelectedSpace.gridPosition - Owner.GetGridSpace().gridPosition;
        for(int i = 0; i <= range; i++)
        {
            var affSpace = GridManager.Instance.GetGridSpace(SelectedSpace.gridPosition + direction * i);
            AddAffectedSpace(affSpace);
        }
    }

    public override void Confirm()
    {
        base.Confirm();

        Debug.Log("AYUDA ME QUIERO MORIR");
        var wizard = Owner as Wizard;
        var AnyEnemyWasAffected = false;
        foreach (var affectedSpace in AffectedSpaces)
        {
            if (affectedSpace.GetEntity() is Enemy enemy)
            {
                AnyEnemyWasAffected = true;
                enemy.BePushed(direction, wizard.pushStrength, wizard.damage, wizard);
            }
        }
        ClearAffectedSpaces();
        ClearSelectableSpaces();
        AudioManager.Instance.PlayAttackSound();
        if (!AnyEnemyWasAffected)
        {
            LogicManager.Instance.PJFinishedMoving();
        }
    }

    protected override bool CanOwnerDoSpecialAbility()
    {
        return true;
    }

    public override void Cancel()
    {
        base.Cancel();
        ClearAffectedSpaces();
        ClearSelectableSpaces();
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
