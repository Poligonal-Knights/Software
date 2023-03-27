using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuSetting : MonoBehaviour
{
    public Canvas a;

    public void setActive()
    {
        a.gameObject.SetActive(true);
    }
    public void setUnActive()
    {
        a.gameObject.SetActive(false);
    }
}
