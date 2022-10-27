using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;

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
            canvas.transform.Find("TurnText").GetComponent<TextMeshProUGUI>().SetText("Tu turno");
        }
        else
        {
            canvas.transform.Find("TurnText").GetComponent<TextMeshProUGUI>().SetText("Turno de la IA");
        }

    }
}
