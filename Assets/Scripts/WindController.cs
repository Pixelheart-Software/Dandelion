using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public float minWind = 0F;
    public float maxWind = 20F;

    internal float direction = 0;

    public static WindController instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
    }

    void Update()
    {
        direction = Random.Range(-minWind, maxWind);
    }
}
