using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float distanceMargin = 0.5F;

    private float playerInitX;
    private float minDistance;
    private float maxDistance;

    private void Awake()
    {
        playerInitX = player.transform.position.x;
        minDistance = transform.position.x - (playerInitX + distanceMargin);
        maxDistance = transform.position.x - (playerInitX - distanceMargin);
    }
    void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }
        float distanceToPlayer = transform.position.x - player.transform.position.x;

        if (distanceToPlayer < minDistance)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + minDistance, transform.position.y, transform.position.z), 0.4F);
        }

        if (distanceToPlayer > maxDistance)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + maxDistance, transform.position.y, transform.position.z), 0.4F);
        }

        transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
    }
}
