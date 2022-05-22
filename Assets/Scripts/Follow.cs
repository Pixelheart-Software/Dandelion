using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public bool followX, followY, followZ;

    public bool keepDistance;
    private Vector3 distance;

    public Transform followed;

    private void Start()
    {
        distance = transform.position - followed.position;

        distance = new Vector3(
            followX ? distance.x : 0,
            followY ? distance.y : 0,
            followZ ? distance.z : 0
        );
    }

    void Update()
    {
        transform.position = new Vector3(
            followX ? followed.transform.position.x : transform.position.x,
            followY ? followed.transform.position.y : transform.position.y,
            followZ ? followed.transform.position.z : transform.position.z
        );

        if (keepDistance)
        {
            transform.position = transform.position + distance;
        }
    }
}
