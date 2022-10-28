using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{

    public GUI UIManager;
    PJ SelectedPJ;

    //Bool to know is somthing as a PJ is moving, attaking, etc. right know
    bool IsSomethingHappening;
    bool CanCancelAction;

    // Start is called before the first frame update
    void Start()
    {

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
            //UIManager.ShowActionCanvas();
            UIManager.ShowPJSelectedUI();
        }
        else if (pj is Enemy)
        {
            UIManager.ShowEnemySelectedUI();
        }
    }

    public PJ GetSelectedPJ()
    {
        return SelectedPJ;
    }

    //Move button
    public void PreviewPJMovement()
    {
        IsSomethingHappening = true;
        SelectedPJ.BFS();
        UIManager.ShowCancelCanvas();
    }

    public bool CanSomethingHappen()
    {
        return !IsSomethingHappening;
    }

    public void CancelAction()
    {
        IsSomethingHappening = false;
        UIManager.ShowPrevoiusCanvas();
        StopPJMovementPreview();
    }

    public void StopPJMovementPreview()
    {
        FindObjectOfType<GridManager>().StopPJMovementPreview();
    }

    public void EntityClicked(Entity entityClicked)
    {
        if (CanSomethingHappen())
        {
            if (entityClicked is PJ)
                SetSelectedPJ(entityClicked as PJ);
            else
            {
                SetSelectedPJ(null);
                UIManager.ShowNothingSelectedUI();
            }
        }

    }
}
