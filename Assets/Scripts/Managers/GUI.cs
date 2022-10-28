using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas turnCanvas;
    public Canvas actionCanvas;

    void Start()
    {

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

    public void ShowActionCanvas()
    {
        actionCanvas.gameObject.SetActive(true);
    }

    public void ShowPJSelectedUI()
    {
        actionCanvas.gameObject.SetActive(true);

    }

    public void ShowEnemySelectedUI()
    {
        actionCanvas.gameObject.SetActive(false);

    }
}
