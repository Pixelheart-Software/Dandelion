using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSeedMovementController : MonoBehaviour
{
    private Rigidbody2D myRigidBody;

    public Transform forcePoint;
    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (forcePoint != null)
        {
            myRigidBody.AddForceAtPosition(new Vector2(WindController.instance.direction + Random.Range(-1f, 1f), 0F) * transform.localScale.x, forcePoint.position, ForceMode2D.Impulse);
        } else
        {
            myRigidBody.AddForce(new Vector2(WindController.instance.direction + Random.Range(-1f, 1f), 0F) * transform.localScale.x, ForceMode2D.Impulse);
        }
        myRigidBody.drag = myRigidBody.drag / transform.localScale.x;
        //myRigidBody.AddTorque(Random.Range(-1f, 1f));
    }
}
