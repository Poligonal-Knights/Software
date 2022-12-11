using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvTutorialManager : MonoBehaviour
{
    //public Canvas initialCanvas;
    //ADEL
    public Canvas welcomeCanvas;
    public Canvas turnosCanvas;
    public Canvas MovementCanvas;
    public Canvas AbilitiesCanvas;
    public Canvas ComboCanvas;
    public Canvas TerminarCanvas;
    
    List<Canvas> allControlCanvas = new List<Canvas>();
    // Start is called before the first frame update
    void Start()
    {
        allControlCanvas = new List<Canvas>();
        allControlCanvas.Add(welcomeCanvas);
        allControlCanvas.Add(turnosCanvas);
        allControlCanvas.Add(MovementCanvas);
        allControlCanvas.Add(AbilitiesCanvas);
        allControlCanvas.Add(ComboCanvas);
        allControlCanvas.Add(TerminarCanvas);
        
    }

    public void ShowThisCanvas(Canvas canvasToShow)
    {
        foreach (Canvas c in allControlCanvas)
        {
            c.gameObject.SetActive(false);
        }
        //previousCanvas = currentCanvas;
        //currentCanvas = canvasToShow;
        canvasToShow.gameObject.SetActive(true);
        Debug.Log("Activado canvas" + canvasToShow.ToString());
        //if(GameManager.turnManager.IsPlayerTurn()) turnCanvas.gameObject.SetActive(true);
        // alwaysActiveCanvas.gameObject.SetActive(true);
    }

    public void HideAllTutorials()
    {
        foreach (Canvas c in allControlCanvas)
        {
            c.gameObject.SetActive(false);
        }

    }
}
