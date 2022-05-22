using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryingController : MonoBehaviour
{

    private SpriteRenderer mySpriteRenderer;
    private Color normalColor;
    public Color deadColor;

    void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = mySpriteRenderer.color;
    }

    private void Start()
    {
        EventBus.Instance.onWaterChange.AddListener(OnWaterChange);
    }

    internal void OnWaterChange(float currentWater)
    {
        mySpriteRenderer.color = Color.Lerp(deadColor, normalColor, currentWater);
    }
}
