using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public bool visibleWhenNotEmpty;
    public Image backgroundImage;

    private void Awake()
    {
        SetVisibility();
    }

    private void SetVisibility()
    {
        if (!visibleWhenNotEmpty)
        {
            return;
        }

        Image image = GetComponent<Image>();
        Color c = image.color;
        c.a = transform.localScale.x <= 0F ? 0 : 1;
        image.color = c;

        c = backgroundImage.color;
        c.a = transform.localScale.x <= 0F ? 0 : 1;
        backgroundImage.color = c;
    }

    public void ChangeValue(float current)
    {
        transform.localScale = new Vector3(Mathf.Clamp01(current), transform.localScale.y, transform.localScale.z);
        SetVisibility();
    }
}
