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
    bool SelectingHability;
    bool HabilityDirectionPreview;
    bool HabilityAreaEffectPreview;
    //bool CanCancelAction;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        HabilityDirectionPreview = false;
        HabilityAreaEffectPreview = false;
        //CanCancelAction = false;
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
            gameManager.UIManager.ShowActionCanvas();
            //gameManager.UIManager.ShowPJSelectedUI();
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
        gameManager.UIManager.ShowPreviewCanvas();
        SelectedPJ.BFS();
    }

    public bool CanSomethingHappen()
    {
        return !IsSomethingHappening;
    }

    public void HabilityButton()
    {
        gameManager.UIManager.ShowHabilitiesCanvas();
        SelectingHability = true;
    }

    public void ConfirmAction()
    {
        if(MovePreview)
        {
            MovePreview = false;

        }

        if(HabilityAreaEffectPreview)
        {
            HabilityAreaEffectPreview = false;
            Debug.Log("Hailidad confirmada");
            (SelectedPJ as Ally).ConfirmHability();
            gameManager.UIManager.ShowEmptyCanvas();
            StopPJHabilityPreview();
        }
    }

    public void CancelAction()
    {
        if (MovePreview)
        {
            MovePreview = false;
            IsSomethingHappening = false;
            StopPJMovementPreview();
            gameManager.UIManager.ShowActionCanvas();
        }

        if (SelectingHability)
        {
            SelectingHability = false;
            gameManager.UIManager.ShowActionCanvas();
        }

        if (HabilityDirectionPreview)
        {
            (SelectedPJ as Ally).StopDoingHability();
            SelectingHability = true;
            StopPJHabilityPreview();
            gameManager.UIManager.ShowHabilitiesCanvas();
        }
    }

    public void StopPJMovementPreview()
    {
        MovePreview = false;
        gameManager.gridManager.StopPJMovementPreview();
        gameManager.UIManager.ShowEmptyCanvas();
    }

    public void StopPJHabilityPreview()
    {
        HabilityDirectionPreview = false;
        HabilityAreaEffectPreview = false;
        gameManager.gridManager.StopPJHabilityPreview();
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
                if (!MovePreview && !SelectingHability && !HabilityDirectionPreview && !HabilityAreaEffectPreview)
                {
                    SetSelectedPJ(entityClicked as PJ);
                }
            }
            else if (entityClicked is Block)
            {
                if (MovePreview)
                {
                    var space = entityClicked.GetGridSpace().neighbors["up"];
                    if (space.IsVisited() && !(space.GetEntity() is PJ))
                    {
                        SelectedPJ.MoveTo(space);
                        StopPJMovementPreview();
                    }
                }
                else if(SelectingHability)
                {

                }
                else if (HabilityDirectionPreview)
                {
                    var space = entityClicked.GetGridSpace().neighbors["up"];
                    if (space.IsSelectable())
                    {
                        space.SetSelected(true);
                        HabilityAreaEffectPreview = true;
                        (SelectedPJ as Ally).SetHabilitySpaceSelected(space);
                    }
                }

                else
                {
                    SetSelectedPJ(null);
                    gameManager.UIManager.ShowNothingSelectedUI();
                }
            }
        }
    }

    public void DoHability(int i)
    {
        if (SelectedPJ is Ally)
        {
            (SelectedPJ as Ally).DoHability(i);
            HabilityDirectionPreview = true;
            SelectingHability = false;
            gameManager.UIManager.ShowPreviewCanvas();
        }
    }
}
