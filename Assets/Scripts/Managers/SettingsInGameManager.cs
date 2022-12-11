using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SettingsInGameManager : MonoBehaviour
{
    public Canvas initialCanvas;
    public Canvas alwaysActive;
    public Canvas turnButton;
    //ADEL
    public Canvas adelCanvas;
    public Canvas adelBasicCanvas;
    public Canvas adelAb1Canvas;
    public Canvas adelAb2Canvas;
    public Canvas adelAb3Canvas;
    //public Canvas adelAb4Canvas;
    public Canvas adelPasiveCanvas;
    //FINADEL
    //MAGNUS
    public Canvas magnusCanvas;
    public Canvas magnusBasicCanvas;
    public Canvas magnusAb1Canvas;
    //public Canvas magnusAb2Canvas;
    public Canvas magnusAb3Canvas;
    public Canvas magnusAb4Canvas;
    public Canvas magnusPasiveCanvas;
    //FINMAGNUS
    //KELSIER
    public Canvas kelsierCanvas;
    public Canvas kelsierBasicCanvas;
    public Canvas kelsierAb1Canvas;
    public Canvas kelsierAb2Canvas;
    public Canvas kelsierAb3Canvas;
    //public Canvas kelsierAb4Canvas;
    public Canvas kelsierPasiveCanvas;
    //FINKELSIER
    //PETRA
    public Canvas petraCanvas;
    public Canvas petraBasicCanvas;
    public Canvas petraAb1Canvas;
    public Canvas petraAb2Canvas;
    //public Canvas petraAb3Canvas;
    public Canvas petraAb4Canvas;
    public Canvas petraPasiveCanvas;
    //FINPETRA
    //ENEMIES
    public Canvas enemyCanvas;
    public Canvas trashmobCanvas;
    public Canvas mageCanvas;
    public Canvas archerCanvas;
    public Canvas tankCanvas;
    public Canvas supportCanvas;
    public Canvas kamikazeCanvas;
    //FINENEMIES
    //CONTROLS
    public Canvas controlCanvas;
    //FINCONTROLS
    List<Canvas> allControlCanvas = new List<Canvas>();
    // Start is called before the first frame update
    void Start()
    {
        allControlCanvas= new List<Canvas>();
        allControlCanvas.Add(initialCanvas);
        allControlCanvas.Add(controlCanvas);
        //A�adir canvas de Magnus
        allControlCanvas.Add(magnusCanvas);
        allControlCanvas.Add(magnusBasicCanvas);
        allControlCanvas.Add(magnusPasiveCanvas);
        allControlCanvas.Add(magnusAb1Canvas);
        //allControlCanvas.Add(magnusAb2Canvas);
        allControlCanvas.Add(magnusAb3Canvas);
        allControlCanvas.Add(magnusAb4Canvas);
        //FIN DE A�adir canvas de Magnus
        //A�adir canvas de Adel
        allControlCanvas.Add(adelCanvas);
        allControlCanvas.Add(adelBasicCanvas);
        allControlCanvas.Add(adelPasiveCanvas);
        allControlCanvas.Add(adelAb1Canvas);
        allControlCanvas.Add(adelAb2Canvas);
        allControlCanvas.Add(adelAb3Canvas);
        //allControlCanvas.Add(adelAb4Canvas);
        //FIN DE A�adir canvas de Adel
        //A�adir canvas de Petra
        allControlCanvas.Add(petraCanvas);
        allControlCanvas.Add(petraBasicCanvas);
        allControlCanvas.Add(petraPasiveCanvas);
        allControlCanvas.Add(petraAb1Canvas);
        allControlCanvas.Add(petraAb2Canvas);
        //allControlCanvas.Add(petraAb3Canvas);
        allControlCanvas.Add(petraAb4Canvas);
        //FIN DE A�adir canvas de Petra
        //A�adir canvas de Kelsier
        allControlCanvas.Add(kelsierCanvas);
        allControlCanvas.Add(kelsierBasicCanvas);
        allControlCanvas.Add(kelsierPasiveCanvas);
        allControlCanvas.Add(kelsierAb1Canvas);
        allControlCanvas.Add(kelsierAb2Canvas);
        allControlCanvas.Add(kelsierAb3Canvas);
        //allControlCanvas.Add(kelsierAb4Canvas);
        //FIN DE A�adir canvas de Kelsier
        //A�adir canvas de enemigos
        allControlCanvas.Add(trashmobCanvas);
        allControlCanvas.Add(mageCanvas);
        allControlCanvas.Add(supportCanvas);
        allControlCanvas.Add(archerCanvas);
        allControlCanvas.Add(tankCanvas);
        allControlCanvas.Add(kamikazeCanvas);
        allControlCanvas.Add(enemyCanvas);
        //FIN DE a�adir canvas de enemigos
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void HideAllSettings()
    {
        foreach (Canvas c in allControlCanvas)
        {
            c.gameObject.SetActive(false);
        }
        alwaysActive.gameObject.SetActive(true);
        turnButton.gameObject.SetActive(true);
        //this.gameObject.SetActive(false);
    }
}
