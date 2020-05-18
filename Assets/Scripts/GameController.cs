using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static GameController Instance;
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;

    enum PageState {
        None,
        Start,
        GameOver,
        CountDown
    }

    int score = 0;
    bool isGameOver = true;

    public bool GameOver {
        get {
            return isGameOver;
        }
    }

    public int Score {
        get {
            return score;
        }
    }

    void Awake () {
        Instance = this;
    }

    void OnEnable() {
        CountdownText.OnCountDownFinished += OnCountDownFinished;
        TapController.OnPlayerScored += OnPlayerScored;
        TapController.OnPlayerDied += OnPlayerDied;
    }

    void OnDisable() {
        CountdownText.OnCountDownFinished -= OnCountDownFinished;
        TapController.OnPlayerScored -= OnPlayerScored;
        TapController.OnPlayerDied -= OnPlayerDied;
    }

    void OnCountDownFinished() {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        isGameOver = false;
    }

    void OnPlayerDied() {
        isGameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");


        if (score > savedScore) {
            PlayerPrefs.SetInt("HighScore", score);
        }

        SetPageState(PageState.GameOver);
    }

    void OnPlayerScored() {
        score++;
        scoreText.text = score.ToString();
    }

    void SetPageState(PageState state) {
        switch(state) {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;
            case PageState.CountDown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
        }
    }

    public void ConfirmGameOver() {
        OnGameOverConfirmed();
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame() {
        SetPageState(PageState.CountDown);
    }
}
