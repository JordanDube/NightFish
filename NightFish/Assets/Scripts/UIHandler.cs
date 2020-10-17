using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject failedPanel;
    [SerializeField] GameObject demonFishPanel;
    [SerializeField] Text fishName;
    [SerializeField] Text length;
    [SerializeField] GameObject newText;
    [SerializeField] Sprite[] fishImages;
    [SerializeField] Image fishCaughtImage;

    public void StartGame()
    {
        fishName.text = "";
        length.text = "";
        newText.SetActive(false);
        fishCaughtImage.sprite = null;
        gameOverPanel.SetActive(false);
        failedPanel.SetActive(false);
        demonFishPanel.SetActive(false);
    }

    public void CaughtFish(string name, float score, bool isHighscore, int imageIndex)
    {
        gameOverPanel.SetActive(true);
        fishName.text = name;
        length.text = "" + score;
        newText.SetActive(isHighscore);
        fishCaughtImage.sprite = fishImages[imageIndex];
        if(name == "demon fish")
        {
            demonFishPanel.SetActive(true);
            //Pause music
        }
    }

    public void FailedCatch()
    {
        failedPanel.SetActive(true);
    }
}
