using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLifeController : MonoBehaviour
{
    public float evaporation = 0.01F;

    public float water = 0.5F;
    public float nutrition = 1.0F;

    private void Start()
    {
        EventBus.Instance.onNutritionChange.Invoke(nutrition);
        EventBus.Instance.onWaterChange.Invoke(water);
    }

    public void AddWater(float amount)
    {
        water += amount;
        water = Mathf.Clamp01(water);
        EventBus.Instance.onWaterChange.Invoke(water);
    }

    public void AddMinerals(float amount)
    {
        nutrition += amount;
        nutrition = Mathf.Clamp01(nutrition);
        EventBus.Instance.onNutritionChange.Invoke(nutrition);
    }

    private void Update()
    {
        water -= evaporation * Time.deltaTime;
        EventBus.Instance.onWaterChange.Invoke(water);
    }
}

