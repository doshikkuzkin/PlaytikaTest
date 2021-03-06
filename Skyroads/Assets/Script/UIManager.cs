﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject congratMessage;
    private Text endScoreText, endHighscoreText, endTimeText, endAsteroidsText;
    private GameManager gameManager;
    private Transform inGameUI, endGameUI, pauseUI;
    private Button restartButton, quitButton;
    private GameObject startPanel;
    private Text timerText, scoreText, hightScoreText, asteroidsText;

    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseQuitButton;
    [SerializeField] private Button pauseSettingsButton;
    [SerializeField] private Button pauseBackButton;
    [SerializeField] private GameObject pauseMain;
    [SerializeField] private GameObject pauseSettings;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundsSlider;

    public String ScoreText
    {
        set
        {
            if (!string.Equals(value, scoreText.text))
                scoreText.text = value;
        }
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        startPanel = transform.Find("StartPanel").gameObject;
        endGameUI = transform.Find("EndGamePanel");
        inGameUI = transform.Find("InGameUI");
        pauseUI = transform.Find("PausePanel");
        timerText = inGameUI.Find("Timer").GetComponent<Text>();
        scoreText = inGameUI.Find("Score").GetComponent<Text>();
        hightScoreText = inGameUI.Find("HighScore").GetComponent<Text>();
        asteroidsText = inGameUI.Find("Asteroids").GetComponent<Text>();
        endScoreText = endGameUI.Find("Score").GetComponent<Text>();
        endAsteroidsText = endGameUI.Find("Asteroids").GetComponent<Text>();
        endTimeText = endGameUI.Find("Time").GetComponent<Text>();
        endHighscoreText = endGameUI.Find("Highscore").GetComponent<Text>();
        restartButton = endGameUI.Find("Restart").GetComponent<Button>();
        quitButton = endGameUI.Find("Quit").GetComponent<Button>();
        congratMessage = endGameUI.Find("Message").gameObject;

        inGameUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(false);

        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(() =>
        {
            PlayerEvents.PlaySound(SoundType.EndGameButtons);
            Application.Quit();
        });
        pauseQuitButton.onClick.AddListener(() =>
        {
            PlayerEvents.PlaySound(SoundType.EndGameButtons);
            Application.Quit();
        });
        pauseResumeButton.onClick.AddListener(ResumeGame);
        
        pauseSettingsButton.onClick.AddListener(() =>
        {
            PlayerEvents.PlaySound(SoundType.SettingButton);
            pauseMain.SetActive(false);
            pauseSettings.SetActive(true);
        });
        
        pauseBackButton.onClick.AddListener(() =>
        {
            PlayerEvents.PlaySound(SoundType.SettingButton);
            pauseMain.SetActive(true);
            pauseSettings.SetActive(false);
            PlayerPrefs.Save();
        });
        
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        soundsSlider.value = PlayerPrefs.GetFloat("SoundsVolume", 1);
        
        musicSlider.onValueChanged.AddListener(PlayerEvents.ChangeMusicVolume);
        soundsSlider.onValueChanged.AddListener(PlayerEvents.ChangeSoundsVolume);
    }

    private void RestartGame()
    {
        PlayerEvents.PlaySound(SoundType.EndGameButtons);
        gameManager.RestartGame();
    }

    //close pause UI and continue game
    private void ResumeGame()
    {
        PlayerEvents.PlaySound(SoundType.EndGameButtons);
        pauseUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        gameManager.PauseGame(false);
    }

    //open pause UI
    public void PauseGame()
    {
        pauseUI.gameObject.SetActive(true);
        inGameUI.gameObject.SetActive(false);
    }

    //close start UI and open player score
    public void StartGame()
    {
        startPanel.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        ScoreText = new Score("0").ToString();
    }

    public void EndGame(int score, int highScore, int asteroids, float time, bool newHighScore)
    {
        //set current score in loose UI
        endScoreText.text = $"Score {score}";
        if (newHighScore)
        {
            //change text if player reaches new highscore
            endHighscoreText.color = Color.green;
            endHighscoreText.text = $"New highscore {highScore}";
            congratMessage.SetActive(true);
        }
        else
        {
            endHighscoreText.text = $"Highscore {highScore}";
            endHighscoreText.color = Color.white;
            congratMessage.SetActive(false);
        }

        endAsteroidsText.text = $"Asteroids {asteroids}";
        endTimeText.text = "Time " + time.ToString("F2") + " sec";
        //open loose UI
        inGameUI.gameObject.SetActive(false);
        endGameUI.gameObject.SetActive(true);
    }

    public void UpdateTimer(float seconds)
    {
        timerText.text = seconds.ToString("F2") + " sec";
    }

    public void UpdateScore(Score score, int highScore, int asteroids)
    {
        ScoreText = score.ToString();
        hightScoreText.text = $"Highscore {highScore}";
        asteroidsText.text = $"Asteroids {asteroids}";
    }
}