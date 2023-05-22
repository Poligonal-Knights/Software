using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelButtonController : MonoBehaviour
{
    public int ID;
    private Animator anim;
    public string HabName;
    public TextMeshProUGUI description;
    public bool selected = false;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        description.text = HabName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        description.text = "";
    }
}
