using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerManager : MonoBehaviour
{
    public static PointerManager Instance { get; private set; }
    private void Awake() => Instance = this;
    Animator animator;
    private Vector3 defaultPos;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = this.transform.position;
        animator = this.GetComponentInChildren<Animator>();
        Debug.LogWarning(animator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPos()
    {
        animator.SetBool("OnObj", false);
        this.transform.position = defaultPos;
    }

    public void OnCharacter(Vector3 a)
    {
        animator.SetBool("OnObj", true);
        this.transform.position = a;
    }
}
