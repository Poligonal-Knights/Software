using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;

using UnityEngine.SceneManagement;

public class NuevoDialogo : MonoBehaviour
{
    [SerializeField]
    private TextAsset _InkJsonFile;
    private Story _StoryScript;

    public float textSpeed;
    public TextMeshProUGUI componenteTexto;
    public TextMeshProUGUI componenteNombre;
    public Image characterIcono;
    public Canvas actionCanvas;
    public Button turnButton;

    public bool final;

    void Start()
    {
        componenteTexto.text = string.Empty;
        componenteNombre.text = string.Empty;
        inicioDial();
    }

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0) || Input.GetKeyUp("space"))
    //    {
    //        proximaLinea();
    //    }
    //}



    void inicioDial()
    {
        UIManager.Instance.ShowTurnButton(false);
        UIManager.Instance.ShowAlwaysCanvas(false);
        actionCanvas.gameObject.SetActive(false);

        _StoryScript = new Story(_InkJsonFile.text);

        _StoryScript.BindExternalFunction("Name", (string charName) => ChangeName(charName));
        _StoryScript.BindExternalFunction("Icon", (string charName) => CharacterIcon(charName));

        proximaLinea();
    } 

    public void proximaLinea() 
    {
        if (_StoryScript.canContinue)
        {
            string text = _StoryScript.Continue(); // Saca la siguiente linea
            text = text?.Trim(); //Quita espacios en blanco entre el texto
            componenteTexto.text = text;
            //StartCoroutine(escribir());
        }
        else
        {
            if (final == false)
            {
                UIManager.Instance.ShowTurnButton(true);
                UIManager.Instance.ShowAlwaysCanvas(true);
                gameObject.SetActive(false);
                turnButton.gameObject.SetActive(true);
                //actionCanvas.gameObject.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene("LvlSelect3D");
            }
        }
    }

    //IEnumerator escribir()
    //{
    //    foreach(char c in _StoryScript.Continue())
    //    {
    //        componenteTexto.text += c;
    //        yield return new WaitForSeconds(textSpeed);
    //    }
    //}

    public void ChangeName(string name)
    {
        string SpeakerName = name;
        componenteNombre.text = SpeakerName;
    }

    public void CharacterIcon(string name)
    {
        var charIcon = Resources.Load<Sprite>("characterIcons/" + name);
        characterIcono.sprite = charIcon;
    }
}
