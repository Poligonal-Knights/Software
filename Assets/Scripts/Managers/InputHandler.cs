using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{

    public LogicManager logicManager;
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))//Click mouse left button
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //hacer cosas de interfaz
            }
            else
            {
                //Ray 
            }
        }

        if (Input.GetKeyUp("a"))
        {
            Debug.Log("a pulsada");

        }
    }

    public void EntityClicked(Entity entityClicked)
    {
        Debug.Log("Ent.Clicked: " + entityClicked);
        if (entityClicked is PJ)
            logicManager.SetSelectedPJ(entityClicked as PJ);
    }
}
