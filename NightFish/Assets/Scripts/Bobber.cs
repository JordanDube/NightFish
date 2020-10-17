using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bobber : MonoBehaviour
{
    private bool canMove = false;
    private Rigidbody2D rb;
    private GameManager gameManager;
    
    [SerializeField] private float movementSpeed = 1.25f;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        StopFalling();
    }

    void Update()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        float h = (Input.GetAxis("Horizontal") * Time.deltaTime) * movementSpeed;
        gameObject.transform.position = new Vector3(transform.position.x + h, transform.position.y, transform.position.z);
    }

    public void StartMove()
    {
        canMove = true;
    }

    public void StartFalling()
    {
        rb.gravityScale = 1f;
    }

    public void StopFalling()
    {
        rb.gravityScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Entered " + other.gameObject.tag);
        switch (other.gameObject.tag)
        {
            case "StartMoving": StartMove();
                break;
            case "ExitScreen": gameManager.ResetGameFail();
                break;
        }
    }

    
}
