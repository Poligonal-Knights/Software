using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int nextScene;
    public static GameManager Instance { get; private set; }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(GameManager.Instance.nextScene);
    }

    Entity[] entities;

    private void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        GridManager.Instance.Init();
        TurnManager.Instance.Init();
    }

    void Init()
    {
        entities = FindObjectsOfType<Entity>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
