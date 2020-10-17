using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFaller : MonoBehaviour
{
    [SerializeField] private float topStopY = 0f;
    [SerializeField] private float bottomStopY = -30f;

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
    }

    public void StartFalling()
    {
        rb.gravityScale = 1f;
    }

    public void StopFalling()
    {
        rb.gravityScale = 0f;
    }
}
