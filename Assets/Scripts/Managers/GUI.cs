using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas emptyCanvas;
    public Canvas turnCanvas;
    public Canvas actionCanvas;
    public Canvas cancelCanvas;

    Canvas currentCanvas;
    Canvas previousCanvas;
    List<Canvas> allCanvas;
    void Start()
    {
        allCanvas = new List<Canvas>();
        //allCanvas.Add(turnCanvas);
        allCanvas.Add(actionCanvas);
        allCanvas.Add(cancelCanvas);
        allCanvas.Add(emptyCanvas);
        currentCanvas = emptyCanvas;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //hacer cosas de interfaz
        }
        else
        {
            //hacer cosas de raycast
        }
    }

    public void ChangeTurn(bool playerTurn)
    {
        if (playerTurn)
        {
            turnCanvas.transform.Find("TurnText").GetComponent<TextMeshProUGUI>().SetText("Tu turno");
        }
        else
        {
            turnCanvas.transform.Find("TurnText").GetComponent<TextMeshProUGUI>().SetText("Turno de la IA");
        }

    }

    void ShowThisCanvas(Canvas canvasToShow)
    {
        foreach(Canvas c in allCanvas)
        {
            c.gameObject.SetActive(false);
        }
        previousCanvas = currentCanvas;
        Debug.Log(previousCanvas);
        currentCanvas = canvasToShow;
        currentCanvas.gameObject.SetActive(true);
    }

    public void ShowPrevoiusCanvas()
    {
        ShowThisCanvas(previousCanvas);
    }

    public void ShowActionCanvas()
    {
        ShowThisCanvas(actionCanvas);
    }

    public void ShowCancelCanvas()
    {
        ShowThisCanvas(cancelCanvas);
    }

    public void ShowEmptyCanvas()
    {
        ShowThisCanvas(emptyCanvas);
    }

    public void ShowPJSelectedUI()
    {
        ShowActionCanvas();
    }

    public void ShowEnemySelectedUI()
    {
        ShowEmptyCanvas();
    }

    public void ShowNothingSelectedUI()
    {
        ShowEmptyCanvas();
    }

}
