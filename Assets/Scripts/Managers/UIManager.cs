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
    //public Canvas MenuInGameCanvas;
    public Canvas actionCanvas;
    public Canvas habilitiesCanvas;
    public Canvas previewCanvas;
    public Canvas ReactionCanvas;
    public Canvas alwaysActiveCanvas;
    public Canvas enemyCanvas;
    public Canvas abilityNonDefined;
    public Canvas SettingsCanvas; 
    
    public Sprite wizardPortrait;
    public Sprite knightPortrait;
    public Sprite priestPortrait;
    public Sprite TrashPortrait;
    public Sprite ArcherPortrait;

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
        allCanvas.Add(enemyCanvas);
        allCanvas.Add(SettingsCanvas);
        //allCanvas.Add(MenuInGameCanvas);
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
        previousCanvas = currentCanvas;
        currentCanvas = canvasToShow;
        currentCanvas.gameObject.SetActive(true);
        //if(GameManager.turnManager.IsPlayerTurn()) turnCanvas.gameObject.SetActive(true);
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

    public void ShowSettings()
    {
        ShowThisCanvas(SettingsCanvas);
        alwaysActiveCanvas.gameObject.SetActive(false);
        turnCanvas.gameObject.SetActive(false);
        //ShowTurnButton();
    }

    public void ShowEnemyCanvas()
    {
        ShowThisCanvas(enemyCanvas);
        
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
        ShowEnemyCanvas();
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

    public void ShowTurnButton(bool set)
    {
        turnCanvas.gameObject.SetActive(set);
    }

    public void HideTurnButton()
    {
        turnCanvas.gameObject.SetActive(false);
    }

    public void ShowSelectedAlly()
    {
        PJ selectedAlly = LogicManager.Instance.GetSelectedPJ();
        if (selectedAlly is Ally ally)
        {
            if (ally.maxHealth == 0)
            {
                actionCanvas.transform.Find("Portrait").Find("HealthBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = ally.health / 10.0f;
            }
            else
            {
                actionCanvas.transform.Find("Portrait").Find("HealthBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount =(float) ally.health / ally.maxHealth;
            }

            actionCanvas.transform.Find("Portrait").Find("HealthBar").Find("Text").GetComponent<TextMeshProUGUI>().SetText(ally.health + "/" + ally.maxHealth);
            
            if (ally.maxEnergy == 0)
            {
                actionCanvas.transform.Find("Portrait").Find("EnergyBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = ally.energy / 10.0f;
            }
            else
            {
                actionCanvas.transform.Find("Portrait").Find("EnergyBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount =(float) ally.energy / ally.maxEnergy;
            }
            actionCanvas.transform.Find("Portrait").Find("EnergyBar").Find("Text").GetComponent<TextMeshProUGUI>().SetText(ally.energy + "/" + ally.maxEnergy);

            if (ally.maxMovement == 0)
            {
                actionCanvas.transform.Find("Portrait").Find("MovBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = ally.energy / 10.0f;
            }
            else
            {
                actionCanvas.transform.Find("Portrait").Find("MovBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = (float)ally.movement / ally.maxMovement;
            }
            actionCanvas.transform.Find("Portrait").Find("MovBar").Find("Text").GetComponent<TextMeshProUGUI>().SetText(ally.movement + "/" + ally.maxMovement);


        }



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

        if (selectedEnemy is TrashMob)
        {
            enemyCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = TrashPortrait;
        }
        else if (selectedEnemy is Archer) 
        {
            enemyCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = ArcherPortrait;
        }

        enemyCanvas.transform.Find("Portrait").Find("EHealthBar").Find("Text").GetComponent<TextMeshProUGUI>().SetText(selectedEnemy.health + "/" + selectedEnemy.maxHealth);
        if (selectedEnemy.maxHealth == 0)
        {
            enemyCanvas.transform.Find("Portrait").Find("EHealthBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = selectedEnemy.health / 10.0f;
        }
        else 
        {
            enemyCanvas.transform.Find("Portrait").Find("EHealthBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = (float) selectedEnemy.health / selectedEnemy.maxHealth;
        }
    }

    public void ShowAlwaysCanvas(bool set)
    {
        alwaysActiveCanvas.gameObject.SetActive(set);
    }

    /*public void ShowMenuIngameCanvas()
    {
        ShowThisCanvas(MenuInGameCanvas);
        alwaysActiveCanvas.gameObject.SetActive(false);
    }

    public void HideMenuIngameCanvas()
    {
        ShowThisCanvas(previousCanvas);
        alwaysActiveCanvas.gameObject.SetActive(true);
    }*/

    public void ShowReactionCanvas(bool b)
    {
        ReactionCanvas.gameObject.SetActive(b);
    }
    public void ShowAbilityNonDefined()
    {
        abilityNonDefined.gameObject.SetActive(true);
    }

    public void HideAbilityNonDefined()
    {
        abilityNonDefined.gameObject.SetActive(false);
    }
}
