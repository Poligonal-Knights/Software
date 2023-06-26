using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScrollButton : MonoBehaviour
{
     List<Button> buttons;

    int HabInUse;
    int firstHab;
    int lastHab;

    Vector3 MainHabPosition;
    Vector3 BottomHabPosition;
    Vector3 TopHabPosition;

    Color NormalColor;
    Color TransparentColor;

    public List<Canvas> RogueSkills;
    public List<Canvas> MageSkills;
    public List<Canvas> KnightSkills;
    public List<Canvas> PriestSkills;

    public List<string> Descripciones; 

    public LogicManager logicManager;
    public UIManager UIManager;
    public TextMeshProUGUI description;
    public Button infoButton;

    // Start is called before the first frame update
    void Start()
    {
        AddDescriptions();

        buttons = new List<Button>(GetComponentsInChildren<Button>());
        lastHab = buttons.Count-1;

        MainHabPosition = new Vector3(0,0,0);
        TopHabPosition = new Vector3(0,130,0);
        BottomHabPosition = new Vector3(0, -130, 0);
        HabInUse = 0;
        firstHab = 0;

        NormalColor = new Color(buttons[lastHab].GetComponent<Image>().color.r, buttons[lastHab].GetComponent<Image>().color.g, buttons[lastHab].GetComponent<Image>().color.b, buttons[lastHab].GetComponent<Image>().color.a);
        TransparentColor = new Color(buttons[lastHab].GetComponent<Image>().color.r, buttons[lastHab].GetComponent<Image>().color.g, buttons[lastHab].GetComponent<Image>().color.b, buttons[lastHab].GetComponent<Image>().color.a / 2);

        ChangeButtons();

    }
  
    public void IncHab()
    {
        if (HabInUse != lastHab)
            HabInUse++;
        else
            HabInUse = firstHab;

        Debug.Log(HabInUse);
        ChangeButtons();
    }

    public void DecHab()
    {
        if (HabInUse != firstHab)
            HabInUse--;
        else
            HabInUse = lastHab;
        ChangeButtons();
    }

    public void ShowInfoCanvas()
    {
        PJ selectedAlly = logicManager.GetSelectedPJ();

        if (selectedAlly is Wizard)
        {
            UIManager.ShowScrollHabCanvas(MageSkills[HabInUse]);
        }
        else if (selectedAlly is Knight)
        {
            UIManager.ShowScrollHabCanvas(KnightSkills[HabInUse]);
        }
        else if (selectedAlly is Priest)
        {
            UIManager.ShowScrollHabCanvas(PriestSkills[HabInUse]);
        }
        else if (selectedAlly is Rogue)
        {
            UIManager.ShowScrollHabCanvas(RogueSkills[HabInUse]);
        }

    }

    public void ShowInfoPanel()
    {
        description.transform.parent.gameObject.SetActive(!description.transform.parent.gameObject.activeSelf);
        infoButton.gameObject.SetActive(!infoButton.gameObject.activeSelf);
    }

    public int GetHabInUse()
    {
        return HabInUse;
    }

    void ChangeButtons()
    {
        foreach(Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }



        buttons[HabInUse].transform.SetLocalPositionAndRotation(MainHabPosition, buttons[HabInUse].transform.rotation);
        buttons[HabInUse].transform.localScale= Vector3.one;
        buttons[HabInUse].GetComponent<Image>().color = NormalColor;
        buttons[HabInUse].gameObject.SetActive(true);
        /*
        if (HabInUse == firstHab)
        {
            buttons[lastHab].transform.SetLocalPositionAndRotation(TopHabPosition, buttons[lastHab].transform.rotation);
            buttons[lastHab].transform.localScale = new Vector3(0.75f,0.75f,0.75f);
            buttons[lastHab].GetComponent<Image>().color = TransparentColor;
            buttons[lastHab].gameObject.SetActive(true);
        }
        else
        {
            buttons[HabInUse - 1].transform.SetLocalPositionAndRotation(TopHabPosition, buttons[HabInUse - 1].transform.rotation);
            buttons[HabInUse - 1].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            buttons[HabInUse - 1].GetComponent<Image>().color = TransparentColor;
            buttons[HabInUse - 1].gameObject.SetActive(true);
        }
            
        if (HabInUse == lastHab)
        {
            buttons[firstHab].transform.SetLocalPositionAndRotation(BottomHabPosition, buttons[firstHab].transform.rotation);
            buttons[firstHab].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            buttons[firstHab].GetComponent<Image>().color = TransparentColor;
            buttons[firstHab].gameObject.SetActive(true);
        }
        else
        {
            buttons[HabInUse + 1].transform.SetLocalPositionAndRotation(BottomHabPosition, buttons[HabInUse +1].transform.rotation);
            buttons[HabInUse + 1].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            buttons[HabInUse + 1].GetComponent<Image>().color = TransparentColor;
            buttons[HabInUse + 1].gameObject.SetActive(true);  
        }
        */
        ChangeDescription();
    }

    void ChangeDescription()
    {
        string desc;

        PJ selectedAlly = logicManager.GetSelectedPJ();
        /*
        if (selectedAlly is Wizard)
        {
            desc = MageSkills[HabInUse].transform.Find("Fondo").Find("Descripción").GetComponentInChildren<TextMeshProUGUI>().text;
            description.text = desc;
        }
        else if (selectedAlly is Knight)
        {
            desc = KnightSkills[HabInUse].transform.Find("Fondo").Find("Descripción").GetComponentInChildren<TextMeshProUGUI>().text;
            description.text = desc;
        }
        else if (selectedAlly is Priest)
        {
            desc = PriestSkills[HabInUse].transform.Find("Fondo").Find("Descripción").GetComponentInChildren<TextMeshProUGUI>().text;
            description.text = desc;
        }
        else if (selectedAlly is Rogue)
        {
            desc = RogueSkills[HabInUse].transform.Find("Fondo").Find("Descripción").GetComponentInChildren<TextMeshProUGUI>().text;
            description.text = desc;

        }
        */
        int index;
        if (selectedAlly is Wizard)
        {
            index = 0;
            desc = Descripciones[index + HabInUse];
            description.text = desc;
        }
        else if (selectedAlly is Knight)
        {
            index = 4;
            desc = Descripciones[index + HabInUse];
            description.text = desc;
        }
        else if (selectedAlly is Priest)
        {
            index = 8;
            desc = Descripciones[index + HabInUse];
            description.text = desc;
        }
        else if (selectedAlly is Rogue)
        {
            index = 12;
            desc = Descripciones[index + HabInUse];
            description.text = desc;

        }
    }

    void AddDescriptions()
    {
        Descripciones.Add("Torrente de aire que afecta a 4 casillas en linea");
        Descripciones.Add("Explosión que afecta a todos los enemigos a su alcance");
        Descripciones.Add("Incremento de velocidad y empuje a todos los aliados");
        Descripciones.Add("Corriente de aire que afecta a 5 casillas en línea que se puede posicionar");

        Descripciones.Add("Golpe de escudo que golpea a las 3 casillas en frente de Magnus");
        Descripciones.Add("Grito de guerra que provoca a los enemigos para que ataquen a Magnus");
        Descripciones.Add("Magnus protege a sus aliados adyacentes y a sí mismo");
        Descripciones.Add("Empujón a un único enemigo a distancia");

        Descripciones.Add("Tirón de una casilla a un único enemigo a distancia");
        Descripciones.Add("Cura a todos los aliados afectados");
        Descripciones.Add("Potenciación de todas las estadísticas de un aliado durente dos turnos");
        Descripciones.Add("Intercambia las posiciones de dos personajes cualesquiera");

        Descripciones.Add("Empujón cuerpo a cuerpo que deja abrojos en el suelo");
        Descripciones.Add("Hace daño y detiene a cualquier enemigo que celisione con ella");
        Descripciones.Add("Envenena a un enemigo, haciendo que reciba daño por cada casilla que se desplaza");
        Descripciones.Add("Aumenta la distancia que son empujados los enemigos que pasan sobre ella");

    }
}
