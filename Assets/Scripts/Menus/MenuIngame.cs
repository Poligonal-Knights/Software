using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuIngame : MonoBehaviour
{
    bool menuAct = true;
    public GameObject boton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void backToMain() {
        SceneManager.LoadScene("MenuP");
    }

    public void hideMen() 
    {
        menuAct = !menuAct;
        if (menuAct==false) 
        {
            boton.SetActive(true);
        } 
        else 
        {
            boton.SetActive(false);
        }
        
       
    }


}
