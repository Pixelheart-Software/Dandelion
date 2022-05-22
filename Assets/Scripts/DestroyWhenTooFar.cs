using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenTooFar : MonoBehaviour
{
    public float maxDistance = 20f;
    public Transform player;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        if (Vector2.Distance(player.position, transform.position) > maxDistance)
        {
            EventBus.Instance.objectDestroyed.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}
