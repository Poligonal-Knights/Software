using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginGame()
    {
        SceneManager.LoadScene(1);
    }
    public void StoryMode()
    {
        SceneManager.LoadScene(2); 
    }
    public void Lvl1()
    {
        SceneManager.LoadScene(3);
    }
    public void Lvl2()
    {
        SceneManager.LoadScene(4);
    }
    public void Lvl3()
    {
        SceneManager.LoadScene(5);
    }
    public void Lvl4()
    {
        SceneManager.LoadScene(6);
    }

}
