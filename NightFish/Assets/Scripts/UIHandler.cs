using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject failedPanel;
    [SerializeField] GameObject demonFishPanel;
    [SerializeField] GameObject startScreen;
    [SerializeField] Text fishName;
    [SerializeField] Text length;
    [SerializeField] Text highscoreText;
    [SerializeField] GameObject newText;
    [SerializeField] Sprite[] fishImages;
    [SerializeField] Image fishCaughtImage;

    bool isDemonFish = false;
    public void StartGame()
    {
        fishName.text = "";
        length.text = "";
        highscoreText.text = "";

        newText.SetActive(false);
        fishCaughtImage.sprite = null;
        gameOverPanel.SetActive(false);
        failedPanel.SetActive(false);
        demonFishPanel.SetActive(false);
        startScreen.SetActive(false);
    }

    public void CaughtFish(string name, float score, bool isHighscore, int imageIndex, int highscore)
    {
        //gameOverPanel.SetActive(true);
        fishName.text = name;
        length.text = ((int)score).ToString();
        highscoreText.text = highscore.ToString();
        newText.SetActive(isHighscore);
        fishCaughtImage.sprite = fishImages[imageIndex];
        if(name == "demon fish")
        {
            isDemonFish = true;
            //Pause music
        }
    }

    public void ShowCaughtPanel()
    {
        if (!isDemonFish)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            demonFishPanel.SetActive(true);
        }
    }
    public void FailedCatch()
    {
        failedPanel.SetActive(true);
    }
}
