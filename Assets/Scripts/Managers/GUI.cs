using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager GameManager;
    public Canvas emptyCanvas;
    public Canvas turnCanvas;
    public Canvas actionCanvas;
    public Canvas cancelCanvas;
    public Canvas alwaysActiveCanvas;

    Canvas currentCanvas;
    Canvas previousCanvas;
    List<Canvas> allCanvas;
    void Start()
    {
        allCanvas = new List<Canvas>();
        allCanvas.Add(actionCanvas);
        allCanvas.Add(cancelCanvas);
        allCanvas.Add(emptyCanvas);
        //allCanvas.Add(turnCanvas);
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
            Debug.Log("Turno aliado comienza");
            alwaysActiveCanvas.transform.Find("TurnText").GetComponent<TextMeshProUGUI>().SetText("Tu turno");
            turnCanvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Turno enemigo comienza");
            alwaysActiveCanvas.transform.Find("TurnText").GetComponent<TextMeshProUGUI>().SetText("Turno de la IA");
            turnCanvas.gameObject.SetActive(false);
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
        //if(GameManager.turnManager.IsPlayerTurn()) turnCanvas.gameObject.SetActive(true);
        // alwaysActiveCanvas.gameObject.SetActive(true);
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
