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
    public Reaction_Ability reactionAbility = new Reaction_Ability();

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
        if (SelectedPJ is Ally ally && ally.getAttackPerformed() == false)
        {
            UIManager.Instance.ShowHabilitiesCanvas();
            SelectingAbility = true;
        }
        else
        {
            Debug.LogWarning("Este aliado ya ha gastado su ataque en este turno");
        }
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
        if (SelectedPJ is Ally ally)
        {
            var newAbility = Ability.GetAbility(ally, i);
            if (newAbility is null)
            {
                Debug.LogError("ERROR AL OBTENER HABILIDAD");
                return;
            }
            if (!newAbility.IsAbilityAvailable())
            {
                Debug.LogWarning("ATAQUE YA GASTADO ESTE TURNO");
                return;
            }
            if (!newAbility.EnougEnergy())
            {
                Debug.LogWarning("ENERGIA NO SUFICIENTE PARA USAR HABILIDAD");
                return;
            }
            currentAbility = newAbility;
            SelectingAbility = false;
            currentAbility.Preview();
            UIManager.Instance.ShowPreviewCanvas();
        }
    }

    public void DoMovementAbility()
    {
        if (SelectedPJ is Ally ally)
        {
            currentAbility = Ability.GetMovementAbility(ally);
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
