using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MovementController : MonoBehaviour
{
    public Vector2 maxJumpForce = new Vector2(2f, 6f);
    public float MoveForce;

    public float MaxDistanceCoCamera = 1.8F;

    private bool CanJump = false;

    private Animator animator;

    private Rigidbody2D myRigidBody;

    private RootsController rootsController;

    private bool canGetNutrition;
    
    void Start()
    {
        animator = GetComponent<Animator>();

        myRigidBody = GetComponent<Rigidbody2D>();

        rootsController = GetComponent<RootsController>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")
            || collision.gameObject.CompareTag("Bug"))
        {
            CanJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")
            || collision.gameObject.CompareTag("Bug"))
        {
            CanJump = false;
        }
    }

    private void FixedUpdate()
    {
        bool isRooted = rootsController.IsRooted();

        if (isRooted)
        {
            myRigidBody.velocity = Vector2.zero;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        } else
        {
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        float hor = Input.GetAxis("Horizontal");
        bool running = hor != 0 && !isRooted;

        var distanceToCamera = Mathf.Abs(transform.position.x - Camera.main.transform.position.x);
        if (distanceToCamera > MaxDistanceCoCamera && hor < 0)
        {
            running = false;
        }
        
        if (running)
        {
            myRigidBody.AddForce(new Vector2(MoveForce, 0F) * Mathf.Sign(hor), ForceMode2D.Force);
            transform.localScale = new Vector2(Mathf.Sign(hor), 1);
        }

        animator.SetBool("running", running && CanJump);

        float vert = Input.GetAxis("Vertical");
        if (!Input.GetKey(KeyCode.DownArrow))
        {
            canGetNutrition = true;
        }

        if (vert != 0 && (CanJump || isRooted))
        {
            bool shouldRoot = Input.GetKey(KeyCode.DownArrow);
            if ((vert <= 0) != isRooted)
            {
                rootsController.RootIn(vert <= 0);
            } else
            if (shouldRoot && isRooted && canGetNutrition)
            {
                rootsController.GetNutrition();
                canGetNutrition = false;
            }
        }

        if (CanJump && vert > 0 && !isRooted)
        {
            myRigidBody.AddForce(maxJumpForce, ForceMode2D.Impulse);
        }
    }
}