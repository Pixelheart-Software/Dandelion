using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private List<Transform> scenery = new List<Transform>();

    private Transform foremost;

    public Transform player;

    private float scenerySize;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        foreach (Transform tr in transform)
        {
            if (tr.CompareTag("Background"))
            {
                scenery.Add(tr);

                if (foremost == null || foremost.position.x < tr.position.x)
                {
                    foremost = tr;
                }
            }
        }

        scenerySize = scenery[0].gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        foreach (Transform tr in scenery)
        {
            if (Mathf.Abs(tr.position.x - player.position.x) > scenerySize * (scenery.Count / 2)
                && tr.position.x < player.position.x)
            {
                tr.position = foremost.position + new Vector3(scenerySize, 0F, 0F);
                foremost = tr;
            }
        }
    }
}
