using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public static LogicManager Instance { get; private set; }

    PJ SelectedPJ;

    bool SelectingHability;
    Hability currentHability;

    private void Awake() => Instance = this;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetSelectedPJ(PJ pj)
    {
        SelectingHability = false;
        SelectedPJ = pj;

        if (pj is Ally)
        {
            UIManager.Instance.ShowActionCanvas();
        }
        else if (pj is Enemy)
        {
            UIManager.Instance.ShowEnemySelectedUI();
        }
    }

    public PJ GetSelectedPJ()
    {
        return SelectedPJ;
    }

    public void HabilityButton()
    {
        UIManager.Instance.ShowHabilitiesCanvas();
        SelectingHability = true;
    }

    public void ConfirmAction()
    {
        if (currentHability is not null && currentHability.IsReadyToConfirm())
        {
            currentHability.Confirm();
            UIManager.Instance.ShowEmptyCanvas();
            currentHability = null;
        }
    }

    public void CancelAction()
    {
        if (SelectingHability)
        {
            SelectingHability = false;
            UIManager.Instance.ShowActionCanvas();
        }
        else
            currentHability?.Cancel();
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
        if (currentHability is not null)
        {
            currentHability.ClickedEntity(entityClicked);
        }
        else
        if (entityClicked is PJ)
        {
            SetSelectedPJ(entityClicked as PJ);
            Debug.Log("SPJ: " + SelectedPJ);
        }
    }

    public void DoHability(int i)
    {
        if (SelectedPJ is Ally)
        {
            currentHability = Hability.GetHability(SelectedPJ, i);
            SelectingHability = false;
            currentHability.Preview();
            UIManager.Instance.ShowPreviewCanvas();
        }
    }

    public void DoMovementHability()
    {
        if (SelectedPJ is Ally)
        {
            currentHability = Hability.GetMovementHability(SelectedPJ);
            currentHability.Preview();
            UIManager.Instance.ShowPreviewCanvas();
        }
    }
}
