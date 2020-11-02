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
    private CircleCollider2D myCollider;
    private GameManager gameManager;
    private CameraFaller cameraFaller;
    UIHandler uiHandler;
    private bool hasChild = false;
    bool badCatch = false;
    
    [SerializeField] private float movementSpeed = 1.25f;
    [SerializeField] Transform startingLocation;
    [SerializeField] float resetSpeed = 5f;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        myCollider = gameObject.GetComponent<CircleCollider2D>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        cameraFaller = FindObjectOfType<CameraFaller>().GetComponent<CameraFaller>();
        uiHandler = FindObjectOfType<UIHandler>().GetComponent<UIHandler>();
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
        rb.gravityScale =  1f;
    }
    public void StopFalling()
    {
        rb.gravityScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "StartMoving": StartMove();
                break;
            case "ExitScreen": gameManager.ResetGameFail(); FailedCatch();
                break;
            case "Fish": if(!hasChild)SetChild(other.gameObject);
                break;
        }
    }

    void FailedCatch()
    {
        reset = true;
        canMove = false;
        StopFalling();
        badCatch = true;
        ResetPosition();
        cameraFaller.ResetPos();
        myCollider.enabled = false;
        gameManager.ResetGameFail();
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
        badCatch = false;
        myCollider.enabled = false;
    }
    
    void ResetPosition()
    {
        gameObject.transform.position = Vector2.MoveTowards(transform.position, startingLocation.position, resetSpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position, startingLocation.position) < .2f)
        {
            reset = false;
            transform.position = startingLocation.position;
            if (hasChild)
                //gameObject.GetComponentInChildren<GameObject>().SetActive(false);
                GameObject.Find(gameObject.GetComponentInChildren<Fish>().name).SetActive(false);
            if(badCatch)
            {
                uiHandler.FailedCatch();
            }
            else
            {
                uiHandler.ShowCaughtPanel();
            }
            hasChild = false;
            myCollider.enabled = true;
            canMove = false;
        }
    }
}
