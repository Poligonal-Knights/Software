using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public static LogicManager Instance { get; private set; }
    PJ SelectedPJ;

    //Bool to know is somthing as a PJ is moving, attaking, etc. right know
    bool IsSomethingHappening;
    bool MovePreview;
    bool SelectingHability;
    bool HabilityDirectionPreview;
    bool HabilityAreaEffectPreview;
    //bool CanCancelAction;

    //Rework
    Hability actualHability;

    private void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
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
            UIManager.Instance.ShowActionCanvas();
            //gameManager.UIManager.ShowPJSelectedUI();
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

    //Move button
    public void PreviewPJMovement()
    {
        MovePreview = true;
        IsSomethingHappening = true;
        UIManager.Instance.ShowPreviewCanvas();
        SelectedPJ.BFS();
    }

    public bool CanSomethingHappen()
    {
        return !IsSomethingHappening;
    }

    public void HabilityButton()
    {
        UIManager.Instance.ShowHabilitiesCanvas();
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
            UIManager.Instance.ShowEmptyCanvas();
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
            UIManager.Instance.ShowActionCanvas();
        }

        if (SelectingHability)
        {
            SelectingHability = false;
            UIManager.Instance.ShowActionCanvas();
        }

        if (HabilityDirectionPreview)
        {
            (SelectedPJ as Ally).StopDoingHability();
            SelectingHability = true;
            StopPJHabilityPreview();
            UIManager.Instance.ShowHabilitiesCanvas();
        }
    }

    public void StopPJMovementPreview()
    {
        MovePreview = false;
        GridManager.Instance.StopPJMovementPreview();
        UIManager.Instance.ShowEmptyCanvas();
    }

    public void StopPJHabilityPreview()
    {
        HabilityDirectionPreview = false;
        HabilityAreaEffectPreview = false;
        GridManager.Instance.StopPJHabilityPreview();
        UIManager.Instance.ShowEmptyCanvas();
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
                    UIManager.Instance.ShowNothingSelectedUI();
                }
            }
        }
    }

    public void DoHability(int i)
    {
        //if (SelectedPJ is Ally)
        //{
        //    (SelectedPJ as Ally).DoHability(i);
        //    HabilityDirectionPreview = true;
        //    SelectingHability = false;
        //    UIManager.Instance.ShowPreviewCanvas();
        //}

        //Rework
        if (SelectedPJ is Ally)
        {
            actualHability = Hability.GetHability(SelectedPJ, i);
            actualHability.Preview();
            HabilityDirectionPreview = true;
            SelectingHability = false;
            UIManager.Instance.ShowPreviewCanvas();
        }
    }
}
