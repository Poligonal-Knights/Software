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
    public Sprite rogueP;
    public Sprite TrashPortrait;
    public Sprite ArcherPortrait;
    public Sprite MagePortrait;
    public Sprite HealerPortrait;
    public Sprite KamikazePortrait;
    public Sprite WardenPortrait;

    Canvas currentCanvas;
    Canvas previousCanvas;
    List<Canvas> allCanvas;

    public PJPointer allyPointer;
    public PJPointer enemyPointer;

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

    public void ShowThisCanvas(Canvas canvasToShow)
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
        
        //PJPointer.Instance.ResetPos();
        allyPointer.ResetPos();
        enemyPointer.ResetPos();
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
        ChangeSelectedHab();
        ShowThisCanvas(habilitiesCanvas);
        ShowTurnButton();
    }

    public void ShowPreviewCanvas()
    {
        PJ selectedAlly = LogicManager.Instance.GetSelectedPJ();
        ShowThisCanvas(previewCanvas);
        UpdateBars(selectedAlly, previewCanvas);
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
        MovePointer(selectedAlly);
        UpdateBars(selectedAlly, actionCanvas);



        
    }

    public void ShowSelectedEnemy()
    {
        PJ selectedEnemy = LogicManager.Instance.GetSelectedPJ();

        MovePointer(selectedEnemy);

        if (selectedEnemy is TrashMob)
        {
            enemyCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = TrashPortrait;
        }
        else if (selectedEnemy is Archer)
        {
            enemyCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = ArcherPortrait;
        }
        else if (selectedEnemy is Mage)
        {
            enemyCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = MagePortrait;
        }
        else if (selectedEnemy is Healer)
        {
            enemyCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = HealerPortrait;
        }
        else if (selectedEnemy is Kamikaze)
        {
            enemyCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = KamikazePortrait;
        }
        else if (selectedEnemy is Warden) 
        {
            enemyCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = WardenPortrait;
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

    public void ChangeSelectedHab() 
    {
        PJ selectedAlly = LogicManager.Instance.GetSelectedPJ();

        if (selectedAlly is Wizard)
        {
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(0).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Empujón de Viento");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(1).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Bomba de Aire");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(2).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Prisa");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(3).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Barrido Vendaval");
            

        }
        else if (selectedAlly is Knight)
        {
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(0).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Escudazo");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(1).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Provocación");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(2).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Cubrir Aliados");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(3).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Lanzar Escudo");
            
        }
        else if (selectedAlly is Priest)
        {
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(0).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Atracción");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(1).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Curación en área");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(2).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Bendición");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(3).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Transposición");
            
        }
        else if (selectedAlly is Rogue) 
        {
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(0).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Abrojo");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(1).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Trampa de Cuerda");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(2).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Cerbatana");
            habilitiesCanvas.transform.Find("ScrollableMenu").Find("Container").GetChild(3).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Trampa de Aceite");
           

        }

        //Viejo//
        /*
        if (selectedAlly is Wizard)
        {
            habilitiesCanvas.transform.GetChild(0).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Empujón de Viento");
            habilitiesCanvas.transform.GetChild(1).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Bomba de Aire");
            habilitiesCanvas.transform.GetChild(2).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Prisa");
            habilitiesCanvas.transform.GetChild(3).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Barrido Vendaval");
            

        }
        else if (selectedAlly is Knight)
        {
            habilitiesCanvas.transform.GetChild(0).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Escudazo");
            habilitiesCanvas.transform.GetChild(1).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Provocación");
            habilitiesCanvas.transform.GetChild(2).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Cubrir Aliados");
            habilitiesCanvas.transform.GetChild(3).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Lanzar Escudo");
            
        }
        else if (selectedAlly is Priest)
        {
            habilitiesCanvas.transform.GetChild(0).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Atracción");
            habilitiesCanvas.transform.GetChild(1).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Curación en área");
            habilitiesCanvas.transform.GetChild(2).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Bendición");
            habilitiesCanvas.transform.GetChild(3).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Transposición");
            
        }
        else if (selectedAlly is Rogue) 
        {
            habilitiesCanvas.transform.GetChild(0).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Abrojo");
            habilitiesCanvas.transform.GetChild(1).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Trampa de Cuerda");
            habilitiesCanvas.transform.GetChild(2).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Cerbatana");
            habilitiesCanvas.transform.GetChild(3).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Trampa de Aceite");
           

        }
         */

        UpdateBars(selectedAlly, habilitiesCanvas);

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

    public void MovePointer(PJ pj)
    {
        if (pj is Ally)
        {
            allyPointer.SetPJ(pj);

        }
        else
        {
            enemyPointer.SetPJ(pj);
        }
    }

    public void UpdateBars(PJ selectedAlly, Canvas targetCanvas) 
    {
        if (selectedAlly is Ally ally)
        {
            if (ally.maxHealth == 0)
            {
                targetCanvas.transform.Find("Portrait").Find("HealthBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = ally.health / 10.0f;
            }
            else
            {
                targetCanvas.transform.Find("Portrait").Find("HealthBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = (float)ally.health / ally.maxHealth;
            }

            targetCanvas.transform.Find("Portrait").Find("HealthBar").Find("Text").GetComponent<TextMeshProUGUI>().SetText(ally.health + "/" + ally.maxHealth);

            if (ally.maxEnergy == 0)
            {
                targetCanvas.transform.Find("Portrait").Find("EnergyBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = ally.energy / 10.0f;
            }
            else
            {
                targetCanvas.transform.Find("Portrait").Find("EnergyBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = (float)ally.energy / ally.maxEnergy;
            }
            targetCanvas.transform.Find("Portrait").Find("EnergyBar").Find("Text").GetComponent<TextMeshProUGUI>().SetText(ally.energy + "/" + ally.maxEnergy);

            if (ally.maxMovement == 0)
            {
                targetCanvas.transform.Find("Portrait").Find("MovBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = ally.energy / 10.0f;
            }
            else
            {
                targetCanvas.transform.Find("Portrait").Find("MovBar").Find("fill").GetComponent<UnityEngine.UI.Image>().fillAmount = (float)ally.movement / ally.maxMovement;
            }
            targetCanvas.transform.Find("Portrait").Find("MovBar").Find("Text").GetComponent<TextMeshProUGUI>().SetText(ally.movement + "/" + ally.maxMovement);


        }

        if (selectedAlly is Wizard)
        {
            targetCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = wizardPortrait;
        }
        else if (selectedAlly is Knight)
        {
            targetCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = knightPortrait;
        }
        else if (selectedAlly is Priest)
        {
            targetCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = priestPortrait;
        }
        else if (selectedAlly is Rogue)
        {
            targetCanvas.transform.Find("Portrait").GetComponent<UnityEngine.UI.Image>().sprite = rogueP;
        }
    }

}
