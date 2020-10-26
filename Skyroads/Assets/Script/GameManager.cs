using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int asteroidsCount;
    private bool gameStarted;
    private int highscore;

    private bool newHighScore;
    private ShipMovement player;
    private int savedHighScore;
    private Score score = 0;
    private TileSpawner tileSpawner;
    private bool timerStarted;

    private float timerTime;
    private UIManager uIManager;

    private void Start()
    {
        tileSpawner = FindObjectOfType<TileSpawner>();
        player = FindObjectOfType<ShipMovement>();
        uIManager = FindObjectOfType<UIManager>();

        PlayerEvents.UpPlayerScore += IncreaseScore;
        PlayerEvents.PlayerDeath += EndGame;
    }

    private void Update()
    {
        if (!gameStarted)
            //is game paused, start on any key
            if (Input.anyKeyDown)
            {
                gameStarted = true;
                StartGame();
            }

        //pause game on Escape
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame(true);
    }

    private void FixedUpdate()
    {
        if (timerStarted)
            UpdateTimer();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    //reload current scene
    public void RestartGame()
    {
        SaveGame();
        SceneManager.LoadScene(0);
    }

    //stop timer when player loose and open loose UI
    private void EndGame()
    {
        timerStarted = false;
        uIManager.EndGame((int) score, highscore, asteroidsCount, timerTime, newHighScore);
    }

    //increase score for moving or passing an asteroid
    private void IncreaseScore(int amount, bool isAsteroid)
    {
        score += amount;
        if (isAsteroid) asteroidsCount++;
        //if new highscore, replace with higher value
        if (score > highscore)
        {
            highscore = (int) score;
            if (!newHighScore) newHighScore = true;
        }

        uIManager.UpdateScore(score, highscore, asteroidsCount);
    }

    public void PauseGame(bool isPaused)
    {
        //stop/start timer
        timerStarted = !isPaused;
        //stop/move tiles
        tileSpawner.IsMovingTiles(!isPaused);
        //stop/move player
        player.Move(!isPaused);
        //open pause UI
        if (isPaused)
            uIManager.PauseGame();
    }

    private void StartGame()
    {
        //Load highscore and update UI
        savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        highscore = savedHighScore;
        uIManager.UpdateScore(score, highscore, asteroidsCount);
        uIManager.StartGame();
        //enable player and tiles movement, start timer
        player.Move(true);
        tileSpawner.IsMovingTiles(true);
        timerStarted = true;
    }

    private void UpdateTimer()
    {
        //count current moving time and update UI
        timerTime += Time.deltaTime;
        uIManager.UpdateTimer(timerTime);
    }

    private void SaveGame()
    {
        //save if it is new highscore
        if (highscore > savedHighScore)
            PlayerPrefs.SetInt("HighScore", highscore);
        PlayerPrefs.Save();
    }
}