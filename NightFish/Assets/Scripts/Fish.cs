using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public string fishName;
    public int fishNum;
    public float fishLength;
    [SerializeField] Sprite underwaterImage;
    //[SerializeField] Sprite caughtImage;
    private bool isCaught = false;
    public void SetCaught(bool caught)
    {
        isCaught = caught;
    }

    public bool GetCaught()
    {
        return isCaught;
    }
}
