using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectGenerator : MonoBehaviour
{
    public GameObject[] prefabs;

    public float minDelay = 5F;
    private float currentDelay = 5F;

    public float minDistance = 2F;
    private Vector2 lastGeneratedPosition;

    private void Start()
    {
        lastGeneratedPosition = transform.position;
    }

    void Update()
    {
        currentDelay -= Time.deltaTime;
        if (currentDelay <= 0)
        {
            if (Vector2.Distance(transform.position, lastGeneratedPosition) > minDistance)
            {
                currentDelay = minDelay + Random.Range(0, 5F);

                var position = transform.position;
                GameObject.Instantiate(RandomPrefab(), position, Quaternion.identity);
                lastGeneratedPosition = position;
            }
        }
    }

    private GameObject RandomPrefab()
    {
        return prefabs[(int)Random.Range(0, prefabs.Length)];
    }
}
