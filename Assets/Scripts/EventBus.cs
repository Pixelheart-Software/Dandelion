using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus : MonoBehaviour
{
    public ParamChangeEvent onNutritionChange;
    public ParamChangeEvent onWaterChange;
    public ParamChangeEvent onSeedTimerChange;
    public ObjectDestroyedEvent objectDestroyed;

    internal static EventBus Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

}

[Serializable]
public class ParamChangeEvent : UnityEvent<float> { }

/**
 * Takes object's tag as argument.
 */
[Serializable]
public class ObjectDestroyedEvent : UnityEvent<GameObject> { }