using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJPointer : MonoBehaviour
{
    PJ pj;
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
        if (pj != null)
        {
            var position = pj.gameObject.transform.position;
            this.transform.position = position + new Vector3(0f, 0.75f, 0f);
        }
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

    public void SetPJ(PJ setTo)
    {
        pj = setTo;
        animator.SetBool("OnObj", pj != null);

    }
}
