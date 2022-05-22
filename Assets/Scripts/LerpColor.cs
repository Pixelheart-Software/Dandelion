using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LerpColor : MonoBehaviour
{
    public Color color1, color2;

    private TextMeshProUGUI myImage;

    void Start()
    {
        myImage = GetComponent<TextMeshProUGUI>();
        myImage.color = color1;
    }

    void Update()
    {
        myImage.color = Color.Lerp(color1, color2, Mathf.PingPong(Time.time, 1));
    }
}
