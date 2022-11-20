using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameManager GameManager;
    public Canvas emptyCanvas;
    public Canvas turnCanvas;
    public Canvas actionCanvas;
    public Canvas habilitiesCanvas;
    public Canvas previewCanvas;
    public Canvas alwaysActiveCanvas;
    public Sprite wizardPortrait;
    public Sprite knightPortrait;
    public Sprite priestPortrait;

    Canvas currentCanvas;
    Canvas previousCanvas;
    List<Canvas> allCanvas;

    private void Awake() => Instance = this;

    void Start()
    {
        allCanvas = new List<Canvas>();
        allCanvas.Add(actionCanvas);
        allCanvas.Add(emptyCanvas);
        allCanvas.Add(habilitiesCanvas);
        allCanvas.Add(previewCanvas);
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
        foreach (Canvas c in allCanvas)
        {
            c.gameObject.SetActive(false);
        }
        currentCanvas = canvasToShow;
        currentCanvas.gameObject.SetActive(true);
        //if(GameManager.turnManager.IsPlayerTurn()) turnCanvas.gameObject.SetActive(true);
        // alwaysActiveCanvas.gameObject.SetActive(true);
    }

    public void ShowActionCanvas()
    {
        ShowThisCanvas(actionCanvas);
        ShowSelectedAlly();
        ShowTurnButton();
    }

    public void ShowEmptyCanvas()
    {
        ShowThisCanvas(emptyCanvas);
        //ShowTurnButton();
    }

    public void ShowHabilitiesCanvas()
    {
        ShowThisCanvas(habilitiesCanvas);
        ShowTurnButton();
    }

    public void ShowPreviewCanvas()
    {
        ShowThisCanvas(previewCanvas);
        HideTurnButton();
    }

    public void ShowPJSelectedUI()
    {
        ShowActionCanvas();
    }

    public void ShowEnemySelectedUI()
    {
        ShowEmptyCanvas();
        ShowSelectedEnemy();
    }

    public void ShowNothingSelectedUI()
    {
        ShowEmptyCanvas();
    }

    public void ShowTurnButton()
    {
        turnCanvas.gameObject.SetActive(true);
    }

    public void HideTurnButton()
    {
        turnCanvas.gameObject.SetActive(false);
    }

    public void ShowSelectedAlly()
    {
        PJ selectedAlly = LogicManager.Instance.GetSelectedPJ();
        

        actionCanvas.transform.Find("Health").GetComponent<TextMeshProUGUI>().SetText(selectedAlly.health + "/" + selectedAlly.maxHealth);

        if (selectedAlly is Wizard)
        {
            actionCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = wizardPortrait;
        }
        else if (selectedAlly is Knight) 
        {
            actionCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = knightPortrait;
        }
        else if (selectedAlly is Priest )
        {
            actionCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = priestPortrait;
        }
    }

    public void ShowSelectedEnemy() 
    {
        PJ selectedEnemy = LogicManager.Instance.GetSelectedPJ();
        string fullName = selectedEnemy.ToString();
        string[] subName = fullName.Split(' ');
        emptyCanvas.transform.Find("EName").GetComponent<TextMeshProUGUI>().SetText(subName[0]);
        emptyCanvas.transform.Find("EHealth").GetComponent<TextMeshProUGUI>().SetText(selectedEnemy.health + "/" + selectedEnemy.maxHealth);

    }

}
