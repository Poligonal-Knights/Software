using System.Collections;
using System.Collections.Generic;
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

    public LogicManager logicManager;
    public UIManager UIManager;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
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

    public void ShowInfo()
    {
        PJ selectedAlly = logicManager.GetSelectedPJ();

        if (selectedAlly is Wizard)
        {
            UIManager.ShowThisCanvas(MageSkills[HabInUse]);
        }
        else if (selectedAlly is Knight)
        {
            UIManager.ShowThisCanvas(KnightSkills[HabInUse]);
        }
        else if (selectedAlly is Priest)
        {
            UIManager.ShowThisCanvas(PriestSkills[HabInUse]);
        }
        else if (selectedAlly is Rogue)
        {
            UIManager.ShowThisCanvas(RogueSkills[HabInUse]);
        }

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

    }
}
