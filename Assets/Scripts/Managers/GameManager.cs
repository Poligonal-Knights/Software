using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public static int currentScene;
    public HashSet<Entity> entities;
    public HashSet<Block> blocks;
    public HashSet<PJ> PJs;
    public HashSet<Ally> allies;
    public HashSet<Enemy> enemies;

    public Canvas dialogoFinal;

    private void Awake()
    {
        Instance = this;
        entities = FindObjectsOfType<Entity>().ToHashSet();
        blocks = FindObjectsOfType<Block>().ToHashSet();
        PJs = FindObjectsOfType<PJ>().ToHashSet();
        allies = FindObjectsOfType<Ally>().ToHashSet();
        enemies = FindObjectsOfType<Enemy>().ToHashSet();
    }

    void Start()
    {
        Init();
        GridManager.Instance.Init();
        TurnManager.Instance.Init();
        currentScene = SceneManager.GetActiveScene().buildIndex;
        if(Camera.main.TryGetComponent(out CameraControls camera)) camera.Init();
    }

    void Init()
    {

    }

    public void RemovePJ(PJ PJToRemove)
    {
        entities.Remove(PJToRemove);
        PJs.Remove(PJToRemove);
        if (PJToRemove is Ally ally)
        {
            allies.Remove(ally);
        }
        else if (PJToRemove is Enemy enemy)
        {
            enemies.Remove(enemy);
        }

        if (enemies.Count <= 0) Victory();
        if (allies.Count <= 0) Defeat();
    }

    void Victory()
    {
        MenuClicker.lvlBeat += 1;
        dialogoFinal.gameObject.SetActive(true);
    }

    void Defeat()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(10);
    }

    void Update()
    {


    }
}
