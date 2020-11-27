using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    [SerializeField] float gravityScale = 1f;

    [SerializeField] Animator[] endingAnimations;
    [SerializeField] GameObject[] turnOffObjects;
    [SerializeField] GameObject[] turnOnObjects;
    bool canMoveToEndDestination = false;
    [SerializeField] Transform cutSceneDestination;
    [SerializeField] float cutSceneSpeed = 1f;
    [SerializeField] float dramaticPause = 2f;
    [SerializeField] float demonTime = 4f;
    [SerializeField] GameObject boat;
    bool dragginBoatDown = false;
    [SerializeField] float draggingSpeed = 20f;
    [SerializeField] Transform underWaterDestination;
    [SerializeField] GameObject blackPanel;
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

        if(canMoveToEndDestination)
        {
            MoveToHead();
        }

        if(dragginBoatDown)
        {
            MoveToSea();
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
        rb.gravityScale =  gravityScale; //1 for normal, -1 for demon
        myCollider.enabled = true;
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
            case "ExitScreen": if (!dragginBoatDown) { gameManager.ResetGameFail(); FailedCatch(); }
                break;
            case "Fish": if(!hasChild)SetChild(other.gameObject);
                break;
            case "CutSceneEnd": CutScene();
                break;
            /*case "DemonHead": FishEnding();
                break;
            case "Boat":
                if (!hasChild) SetChild(other.gameObject); DemonEnding();
                break;*/
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
        canMove = false;
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
            //canMove = false;
        }
    }

    void CutScene()
    {
        canMove = false;
        canMoveToEndDestination = true;
        StopFalling();
    }
    
    void MoveToHead()
    {
        gameObject.transform.position =
               Vector2.MoveTowards(transform.position, cutSceneDestination.position, cutSceneSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, cutSceneDestination.position) < 0.8f)
        {
            print("Met distance");
            canMoveToEndDestination = false;
            if(gravityScale == 1f)
            {
                print("FishEndingTry");
                FishEnding();
            }
            else if(gravityScale == -1)
            { print("DemonEndingTry"); DemonEnding(); }
        }
        print(Vector2.Distance(transform.position, cutSceneDestination.position));
    }

    void MoveToSea()
    {
        gameObject.transform.position =
               Vector2.MoveTowards(transform.position, underWaterDestination.position, draggingSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, underWaterDestination.position) < 0.8f)
        {
            dragginBoatDown = false;
            uiHandler.GameDone();
            Destroy(gameObject);
        }
    }
    void FishEnding()
    {
        CycleThroughObjects();
        StartCoroutine(DemonWakes());
    }

    IEnumerator DemonWakes()
    {
        yield return new WaitForSeconds(dramaticPause);
        uiHandler.DemonAwakes();
        yield return new WaitForSeconds(demonTime);
        blackPanel.SetActive(true);
        SceneManager.LoadScene(1);
    }
    void CycleThroughObjects()
    {
        foreach (Animator anim in endingAnimations)
        {
            anim.enabled = false;
        }
        foreach (GameObject obj in turnOffObjects)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in turnOnObjects)
        {
            obj.SetActive(true);
        }
    }

    void DemonEnding()
    {
        CycleThroughObjects();
        boat.transform.SetParent(this.gameObject.transform);
        StartCoroutine(BoatDrags());
    }

    IEnumerator BoatDrags()
    {
        yield return new WaitForSeconds(dramaticPause);
        dragginBoatDown = true;
    }
}
