using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public GameManager gameManager;
    PJ SelectedPJ;

    //Bool to know is somthing as a PJ is moving, attaking, etc. right know
    bool IsSomethingHappening;
    bool MovePreview;
    bool HabilityDirectionPreview;
    bool HabilityAreaEfectPreview;
    bool CanCancelAction;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSelectedPJ(PJ pj)
    {
        Debug.Log("SelectedPJ changed!");
        SelectedPJ = pj;

        if (pj is Ally)
        {
            //gameManager.UIManager.ShowActionCanvas();
            gameManager.UIManager.ShowPJSelectedUI();
        }
        else if (pj is Enemy)
        {
            gameManager.UIManager.ShowEnemySelectedUI();
        }
    }

    public PJ GetSelectedPJ()
    {
        return SelectedPJ;
    }

    //Move button
    public void PreviewPJMovement()
    {
        MovePreview = true;
        IsSomethingHappening = true;
        SelectedPJ.BFS();
        gameManager.UIManager.ShowCancelCanvas();
    }

    public bool CanSomethingHappen()
    {
        return !IsSomethingHappening;
    }

    public void HabilityButton()
    {
        gameManager.UIManager.ShowHabilitiesCanvas();
    }

    public void CancelAction()
    {
        if (MovePreview) MovePreview = false;
        IsSomethingHappening = false;
        gameManager.UIManager.ShowPrevoiusCanvas();
        StopPJMovementPreview();
    }

    public void StopPJMovementPreview()
    {
        MovePreview = false;
        FindObjectOfType<GridManager>().StopPJMovementPreview();
        gameManager.UIManager.ShowEmptyCanvas();
    }

    public void PJFinishedMoving()
    {
        if (gameManager.turnManager.IsPlayerTurn())
        {
            gameManager.UIManager.ShowActionCanvas();
        }
    }

    public void EntityClicked(Entity entityClicked)
    {
        //if (CanSomethingHappen())
        if (true)
        {
            if (entityClicked is PJ)
            {
                SetSelectedPJ(entityClicked as PJ);
            }
            else if (entityClicked is Block && MovePreview)
            {
                var space = entityClicked.GetGridSpace().neighbors["up"];
                if (space.IsVisited() && !(space.GetEntity() is PJ))
                {
                    SelectedPJ.MoveTo(space);
                    StopPJMovementPreview();
                }
            }
            else
            {
                SetSelectedPJ(null);
                gameManager.UIManager.ShowNothingSelectedUI();
            }
        }
    }

    public void DoHability(int i)
    {
        if(SelectedPJ is Ally)
        {
            (SelectedPJ as Ally).DoHability(i);
            gameManager.UIManager.ShowCancelCanvas();
        }
    }
}
