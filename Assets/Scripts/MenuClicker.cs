using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuClicker : MonoBehaviour
{
    public static MenuClicker Instance { get; private set; }
    private void Awake() => Instance = this;
    public static int lvlBeat = 0;
    public Canvas selectedCanvas;
    public GameObject pointer;
    public GameObject lvl1;
    public GameObject lvl2;
    public GameObject lvl3;
    public GameObject lvl4;
    public GameObject lvl5;
    public GameObject lvl6;
    public GameObject lvl7;
    public GameObject lvl8;
    public Sprite a,b,c,d;

    Vector3 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        oldPos = pointer.transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out hit)) 
            {
                if (hit.transform != null && hit.transform.gameObject.CompareTag("SelectorLvl")) 
                {
                    Vector3 pos = new Vector3(hit.transform.position.x, hit.transform.position.y + 1.00f, hit.transform.position.z);
                    pointer.transform.SetPositionAndRotation(pos, hit.transform.rotation);

                    if (GameObject.ReferenceEquals(hit.transform.gameObject,lvl1))
                    {
                        selectedCanvas.transform.Find("Fondo").GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Mundo 1 Nivel 1");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Pequeño");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = a;
                        selectedCanvas.transform.Find("Fondo").GetChild(2).GetComponent<TextMeshProUGUI>().SetText("2");
                        selectedCanvas.transform.Find("Fondo").GetChild(3).GetComponent<TextMeshProUGUI>().SetText("7");
                        IsPlayeable(0);
                        selectedCanvas.transform.Find("Fondo").Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl1());
                    }else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl2))
                    {
                        selectedCanvas.transform.Find("Fondo").GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Mundo 1 Nivel 2");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Pequeño");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = a;
                        selectedCanvas.transform.Find("Fondo").GetChild(2).GetComponent<TextMeshProUGUI>().SetText("2");
                        selectedCanvas.transform.Find("Fondo").GetChild(3).GetComponent<TextMeshProUGUI>().SetText("5");
                        IsPlayeable(1);
                        selectedCanvas.transform.Find("Fondo").Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl2());
                    }
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl3))
                    {
                        selectedCanvas.transform.Find("Fondo").GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Mundo 1 Nivel 3");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Pequeño");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = b;
                        selectedCanvas.transform.Find("Fondo").GetChild(2).GetComponent<TextMeshProUGUI>().SetText("4");
                        selectedCanvas.transform.Find("Fondo").GetChild(3).GetComponent<TextMeshProUGUI>().SetText("11");
                        IsPlayeable(2);
                        selectedCanvas.transform.Find("Fondo").Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl3());
                    }
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl4))
                    {
                        selectedCanvas.transform.Find("Fondo").GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Mundo 1 Nivel 4");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Mediano");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = b;
                        selectedCanvas.transform.Find("Fondo").GetChild(2).GetComponent<TextMeshProUGUI>().SetText("4");
                        selectedCanvas.transform.Find("Fondo").GetChild(3).GetComponent<TextMeshProUGUI>().SetText("11");
                        IsPlayeable(3);
                        selectedCanvas.transform.Find("Fondo").Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl4());
                    }//
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl5))
                    {
                        selectedCanvas.transform.Find("Fondo").GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Mundo 1 Nivel 5");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Mediano");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = c;
                        selectedCanvas.transform.Find("Fondo").GetChild(2).GetComponent<TextMeshProUGUI>().SetText("4");
                        selectedCanvas.transform.Find("Fondo").GetChild(3).GetComponent<TextMeshProUGUI>().SetText("12");
                        IsPlayeable(4);
                        selectedCanvas.transform.Find("Fondo").Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl5());
                    }
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl6))
                    {
                        selectedCanvas.transform.Find("Fondo").GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Mundo 1 Nivel 6");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Grande");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = c;
                        selectedCanvas.transform.Find("Fondo").GetChild(2).GetComponent<TextMeshProUGUI>().SetText("4");
                        selectedCanvas.transform.Find("Fondo").GetChild(3).GetComponent<TextMeshProUGUI>().SetText("13");
                        IsPlayeable(5);
                        selectedCanvas.transform.Find("Fondo").Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl6());
                    }
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl7))
                    {
                        selectedCanvas.transform.Find("Fondo").GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Mundo 1 Nivel 7");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Grande");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = d;
                        selectedCanvas.transform.Find("Fondo").GetChild(2).GetComponent<TextMeshProUGUI>().SetText("4");
                        selectedCanvas.transform.Find("Fondo").GetChild(3).GetComponent<TextMeshProUGUI>().SetText("13");
                        IsPlayeable(6);
                        selectedCanvas.transform.Find("Fondo").Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl7());
                    }
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl8))
                    {
                        selectedCanvas.transform.Find("Fondo").GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Mundo 1 Nivel 8");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Grande");
                        selectedCanvas.transform.Find("Fondo").GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = d; 
                        selectedCanvas.transform.Find("Fondo").GetChild(2).GetComponent<TextMeshProUGUI>().SetText("4");
                        selectedCanvas.transform.Find("Fondo").GetChild(3).GetComponent<TextMeshProUGUI>().SetText("13");
                        IsPlayeable(7);
                        selectedCanvas.transform.Find("Fondo").Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl8());
                    }



                    selectedCanvas.gameObject.SetActive(true);

                }
                else 
                {
                    selectedCanvas.gameObject.SetActive(false);
                    pointer.transform.SetPositionAndRotation(oldPos, hit.transform.rotation);
                }
            }
        }
        
    }

    public void IsPlayeable(int lvl) 
    {
        
        if (lvl > lvlBeat)
        {
            selectedCanvas.transform.Find("Fondo").Find("SiBt").GetComponent<Button>().interactable = false;
            
        }
        else 
        {
            selectedCanvas.transform.Find("Fondo").Find("SiBt").GetComponent<Button>().interactable = true;
            
        }
    }

    
    

}
