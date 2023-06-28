using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthBar : MonoBehaviour
{
    public PJ pj;
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        pj.HealthChanged += OnHealthChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnHealthChanged()
    {
        image.fillAmount = (float)pj.health / pj.maxHealth;
    }
}
