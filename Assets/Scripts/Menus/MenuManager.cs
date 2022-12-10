using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MenuManager Instance { get; private set; }
    private void Awake() => Instance = this;
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
    public void Lvl5()
    {
        SceneManager.LoadScene(7);
    }
    public void Lvl6()
    {
        SceneManager.LoadScene(8);
    }
    public void Lvl7()
    {
        SceneManager.LoadScene(9);
    }
    public void Lvl8()
    {
        SceneManager.LoadScene(10);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void BackToMSelect()
    {
        SceneManager.LoadScene(1);
    }

    

    public void CloseLvlCanvas()
    {
        MenuClicker.Instance.selectedCanvas.gameObject.SetActive(false);
        MenuClicker.Instance.pointer.transform.SetPositionAndRotation(new Vector3(0,50,0),new Quaternion());
    }

}
