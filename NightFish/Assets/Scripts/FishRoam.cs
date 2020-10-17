using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishRoam : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;
    private Transform moveSpot;
    [SerializeField] private Transform moveSpot1;
    [SerializeField] private Transform moveSpot2;
    
    private float waitTime;
    public float startWaitTime = 5f;
    private void Start()
    {
        waitTime = startWaitTime;
        moveSpot.position = moveSpot1.position;
    }

    private void Update()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                waitTime = startWaitTime;
                ToggleMove();
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void ToggleMove()
    {
        if (Vector2.Distance(transform.position, moveSpot1.position) < 0.2f)
        {
            moveSpot.position = moveSpot2.position;
        }
        else
        {
            moveSpot.position = moveSpot1.position;
        }
    }
}

