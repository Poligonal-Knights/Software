using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerManager : MonoBehaviour
{
    public static PointerManager Instance { get; private set; }
    private void Awake() => Instance = this;

    private Vector3 defaultPos;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPos()
    {
        this.transform.position = defaultPos;
    }

    public void OnCharacter(Vector3 a)
    {
        this.transform.position = a;
    }
}
