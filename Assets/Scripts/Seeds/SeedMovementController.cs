using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedMovementController : MonoBehaviour
{
    public Vector2 force = Vector2.right * 10;

    private Rigidbody2D myRigidBody;    
    
    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        myRigidBody.AddForce((force + new Vector2(Random.Range(0F, 10F), 0F)) * Time.deltaTime);
    }
}
