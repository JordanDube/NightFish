using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameStarted = false;

    private CameraFaller cameraFaller;

    private Bobber bobber;
    private UIHandler uiHandler;
    [SerializeField] bool[] fishTracker;
    [SerializeField] GameObject[] fish;
    [SerializeField] GameObject demonFish;
    bool hasDemonFish = false;
    float highScore;
    bool hasFish = false;
    int level = 0;

    private void Awake()
    {
        //Find Save file
        cameraFaller = FindObjectOfType<CameraFaller>().GetComponent<CameraFaller>();
        bobber = FindObjectOfType<Bobber>().GetComponent<Bobber>();
        uiHandler = FindObjectOfType<UIHandler>().GetComponent<UIHandler>();
    }

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
        uiHandler.StartGame();
        LoadFish();
        gameStarted = true;
    }

    void LoadFish()
    {
        for(int i = 0; i < fishTracker.Length; i++)
        {
            fish[i].SetActive(fishTracker[i]);
            if(fishTracker[i])
            {
                hasFish = true;
            }
        }
        if(!hasFish)
        {
            demonFish.SetActive(true);
        }
    }
    public void ResetGameFail()
    {
        //uiHandler.FailedCatch();
    }

    public void CaughtFish(int fishNumber, float fishLength, string fishName)
    {
        if(fishLength > highScore)
        {
            highScore = fishLength;
            uiHandler.CaughtFish(fishName, fishLength, true, fishNumber, (int)highScore);
        }
        else
        {
            uiHandler.CaughtFish(fishName, fishLength, false, fishNumber, (int)highScore);
        }
        fishTracker[fishNumber] = false;
        gameStarted = false;
    }


}
