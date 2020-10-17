using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameStarted = false;

    private CameraFaller cameraFaller;

    private Bobber bobber;

    private void Awake()
    {
        //Find Save file
        cameraFaller = FindObjectOfType<CameraFaller>().GetComponent<CameraFaller>();
        bobber = FindObjectOfType<Bobber>().GetComponent<Bobber>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !gameStarted)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        cameraFaller.StartFalling();
        bobber.StartFalling();
    }
    
    public void ResetGameFail()
    {
        
    }
}
