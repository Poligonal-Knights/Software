using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public static LogicManager Instance { get; private set; }

    PJ SelectedPJ;

    bool SelectingAbility;
    public Ability currentAbility;
    public Reaction_Ability reactionAbility = new Reaction_Ability(null);

    private void Awake() => Instance = this;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetSelectedPJ(PJ pj)
    {
        SelectingAbility = false;
        SelectedPJ = pj;

        if (TurnManager.Instance.IsPlayerTurn())
        {
            if (pj is Ally)
            {
                UIManager.Instance.ShowActionCanvas();
            }
            else if (pj is Enemy)
            {
                UIManager.Instance.ShowEnemySelectedUI();
            }
        }
    }

    public PJ GetSelectedPJ()
    {
        return SelectedPJ;
    }

    public void AbilityButton()
    {
        UIManager.Instance.ShowHabilitiesCanvas();
        SelectingAbility = true;
    }

    public void ConfirmAction()
    {
        if (currentAbility is not null && currentAbility.IsReadyToConfirm())
        {
            currentAbility.Confirm();
            UIManager.Instance.ShowEmptyCanvas();
            currentAbility = null;
        }
    }

    public void CancelAction()
    {
        if (SelectingAbility)
        {
            SelectingAbility = false;
            UIManager.Instance.ShowActionCanvas();
        }
        else if (currentAbility is not null)
        {
            currentAbility.Cancel();

        }
    }

    public void PJFinishedMoving()
    {
        if (TurnManager.Instance.IsPlayerTurn())
        {
            UIManager.Instance.ShowActionCanvas();
        }
    }

    public void EntityClicked(Entity entityClicked)
    {
        if (currentAbility is not null)
        {
            currentAbility.ClickedEntity(entityClicked);
        }
        else
        if (entityClicked is PJ)
        {
            SetSelectedPJ(entityClicked as PJ);
        }
    }

    public void DoAbility(int i)
    {
        if (SelectedPJ is Ally)
        {
            currentAbility = Ability.GetAbility(SelectedPJ, i);
            SelectingAbility = false;
            currentAbility?.Preview();
            UIManager.Instance.ShowPreviewCanvas();
        }
    }

    public void DoMovementAbility()
    {
        if (SelectedPJ is Ally)
        {
            currentAbility = Ability.GetMovementAbility(SelectedPJ);
            currentAbility.Preview();
            UIManager.Instance.ShowPreviewCanvas();
        }
    }

    public void ComboButton()
    {
        reactionAbility?.Confirm();
    }

    public void NahButton()
    {
        reactionAbility?.Cancel();
    }
}
