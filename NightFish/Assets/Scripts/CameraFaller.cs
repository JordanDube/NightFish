using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFaller : MonoBehaviour
{
    [SerializeField] private float topStopY = 0f;
    [SerializeField] private float bottomStopY = -30f;
    [SerializeField] Transform startingLocation;
    [SerializeField] float resetSpeed = 5f;

    bool canReset = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        StopFalling();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= bottomStopY)
        {
            StopFalling();
        }

        if(canReset)
        {

        }
    }

    public void StartFalling()
    {
        rb.gravityScale = 1f;
    }

    public void StopFalling()
    {
        rb.gravityScale = 0f;
    }

    private void ResetPosition()
    {
        gameObject.transform.position = Vector2.MoveTowards(transform.position, startingLocation.position, resetSpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position, startingLocation.position) < 0.2f)
        {
            canReset = false;
            transform.position = startingLocation.position;
        }
    }

    public void ResetPos()
    {
        canReset = true;
        StopFalling();
    }
}
