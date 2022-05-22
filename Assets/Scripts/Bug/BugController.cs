using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugController : MonoBehaviour
{
    public float velocity;
    internal bool eating;
    private Transform player;

    public float attackPower = 0.1F;

    private Rigidbody2D myRigidBody;

    public float directionSwitchingTime = 0.5f;
    private float directionSwitching = 0F;
    private float currentDirection;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        myRigidBody = GetComponent<Rigidbody2D>();

        currentDirection = Mathf.Sign(transform.localScale.x);
    }

    void FixedUpdate()
    {
        if (eating)
        {
            myRigidBody.velocity = Vector2.zero;
            return;
        }

        if (directionSwitching > 0)
        {
            directionSwitching -= Time.deltaTime;
        } else
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);

            if (currentDirection != direction)
            {
                directionSwitching = directionSwitchingTime;
            }

            myRigidBody.velocity = new Vector2(velocity * direction, 0F);

            transform.localScale = new Vector2(direction, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (eating) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            eating = true;
            PlayerLifeController lifeController = collision.gameObject.GetComponent<PlayerLifeController>();
            lifeController.AddMinerals(-attackPower);
        }
    }
}
