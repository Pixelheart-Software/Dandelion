using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedFadeOut : MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var color = mySpriteRenderer.color;

        color.a = Mathf.Lerp(color.a, 0, 0.6F * Time.deltaTime);

        if (color.a < 0.1F)
        {
            Destroy(gameObject);
        }

        mySpriteRenderer.color = color;
    }
}
