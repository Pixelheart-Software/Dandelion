using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutritionResourceController : MonoBehaviour
{
    public float singleNutritionUnit = 0.1F;

    public NutritionType type;

    private float nutritionUnitsLeft;

    private void Start()
    {
        nutritionUnitsLeft = singleNutritionUnit * 5F;
    }

    public Nutrition GetNutrition()
    {
        if (nutritionUnitsLeft < singleNutritionUnit)
        {
            GameObject.Destroy(gameObject);
            return new Nutrition(type, nutritionUnitsLeft);
        }

        nutritionUnitsLeft -= singleNutritionUnit;
        transform.localScale = transform.localScale * Mathf.Clamp01(0.2F + (nutritionUnitsLeft / (singleNutritionUnit * 5F)));
        return new Nutrition(type, singleNutritionUnit);
    }
}

public enum NutritionType
{
    Water, Minerals
}

public struct Nutrition
{
    public NutritionType type { get; }
    public float amount { get;  }



    public Nutrition(NutritionType type, float amount)
    {
        this.type = type;
        this.amount = amount;
    }
}