using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bobber : MonoBehaviour
{
    private bool canMove = false;
    private bool reset = false;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private CameraFaller cameraFaller;
    private bool hasChild = false;
    
    [SerializeField] private float movementSpeed = 1.25f;
    [SerializeField] Transform startingLocation;
    [SerializeField] float resetSpeed = 5f;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        cameraFaller = FindObjectOfType<CameraFaller>().GetComponent<CameraFaller>();
        StopFalling();
    }

    void Update()
    {
        if (canMove)
        {
            Move();
        }

        if(reset)
        {
            ResetPosition();
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
            case "Fish": SetChild(other.gameObject);
                break;
        }
    }

    void SetChild(GameObject other)
    {
        other.transform.SetParent(this.gameObject.transform);
        other.GetComponent<FishRoam>().StopMovement();
        reset = true;
        canMove = false;
        StopFalling();
        hasChild = true;
        Fish fish = other.gameObject.GetComponent<Fish>();
        gameManager.CaughtFish(fish.fishNum, fish.fishLength, fish.fishName);
        cameraFaller.ResetPos();
    }
    
    void ResetPosition()
    {
        gameObject.transform.position = Vector2.MoveTowards(transform.position, startingLocation.position, resetSpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position, startingLocation.position) < .2f)
        {
            reset = false;
            canMove = true;
            transform.position = startingLocation.position;
            Destroy(gameObject.GetComponentInChildren<GameObject>());
        }
    }
}
