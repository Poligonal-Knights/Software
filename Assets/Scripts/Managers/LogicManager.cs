using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{

    public GUI UIManager;
    PJ SelectedPJ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectedPJ(PJ pj)
    {
        Debug.Log("SelectedPJ changed!");
        SelectedPJ = pj;
        UIManager.ShowActionCanvas();
    }

    public PJ GetSelectedPJ()
    {
        return SelectedPJ;
    }
}
