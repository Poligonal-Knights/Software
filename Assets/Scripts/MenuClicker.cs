using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuClicker : MonoBehaviour
{
    public static MenuClicker Instance { get; private set; }
    private void Awake() => Instance = this;
    
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
                    Vector3 pos = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.65f, hit.transform.position.z);
                    pointer.transform.SetPositionAndRotation(pos, hit.transform.rotation);

                    if (GameObject.ReferenceEquals(hit.transform.gameObject,lvl1))
                    {
                        
                        selectedCanvas.transform.Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl1());
                    }else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl2))
                    {
                        selectedCanvas.transform.Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl2());
                    }
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl3))
                    {
                        selectedCanvas.transform.Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl3());
                    }
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl4))
                    {
                        selectedCanvas.transform.Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl4());
                    }//
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl5))
                    {
                        selectedCanvas.transform.Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl5());
                    }
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl6))
                    {
                        selectedCanvas.transform.Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl6());
                    }
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl7))
                    {
                        selectedCanvas.transform.Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl7());
                    }
                    else if (GameObject.ReferenceEquals(hit.transform.gameObject, lvl8))
                    {
                        selectedCanvas.transform.Find("SiBt").GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.Lvl8());
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
}
