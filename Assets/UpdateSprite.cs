using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite frontSprite, backSprite;

    SpriteRenderer spriteRenderer;
    Vector2 cameraVector;
    Vector2 initOrientation;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 camVector = Camera.main.transform.position - gameObject.transform.position;
        cameraVector = new Vector2(camVector.x, camVector.z);
        initOrientation = cameraVector;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camVector = Camera.main.transform.position - gameObject.transform.position;
        cameraVector = new Vector2(camVector.x, camVector.z);

        var angle = Vector2.SignedAngle(initOrientation, cameraVector);
        spriteRenderer.sprite = Math.Abs(angle) < 90.0f ? frontSprite : backSprite;
        spriteRenderer.flipX = angle > 0 ? true : false;
    }
}
