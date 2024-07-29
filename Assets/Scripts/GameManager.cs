using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player Player;
    public Text ScoreText;
    public GameObject GameOverText;
    public GameObject StartScreenPrefab;
    public GameObject RestartGameButton;
    public GameObject RenewButton;
    public GameObject GoldMedal;
    public GameObject SilverMeal;
    public GameObject Table;
    public Text HighScoreText;
    public Text FinalScoreText;
    public Text FPSText;

    private int _score;
    private int _highscore;
    public ObjectPool _pool;
    private bool _gameStarted;
    private GameObject startScreenInstance;

    private void PresetActive()
    {
        GameOverText.SetActive(false);
        Table.SetActive(false);
        GoldMedal.SetActive(false);
        SilverMeal.SetActive(false);
        FinalScoreText.gameObject.SetActive(false);
        HighScoreText.gameObject.SetActive(false);
        RestartGameButton.SetActive(false);
        RenewButton.SetActive(false);
    }

    private void Awake()
    {
        PresetActive();
        _highscore = PlayerPrefs.GetInt("HighScore");
        Application.targetFrameRate = 60;
        _gameStarted = true;
        Pause();

        if (StartScreenPrefab != null)
        {
            startScreenInstance = Instantiate(StartScreenPrefab);
        }
    }

    private void UpdateHighScore()
    {
        if (_score > _highscore)
        {
            _highscore = _score;
            PlayerPrefs.SetInt("HighScore", _highscore);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        if (_gameStarted && (Input.GetMouseButtonDown(0) || Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)))
        {
            Play();
        }

        UpdateHighScore();
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateFPS), 0.5f, 0.5f);

        if (StartScreenPrefab != null && startScreenInstance == null)
        {
            startScreenInstance = Instantiate(StartScreenPrefab);
        }
    }

    private void UpdateFPS()
    {
        var fpsCalculation = 1 / Time.unscaledDeltaTime;
        var fps = (int)fpsCalculation;
        FPSText.text = fps + " FPS";
    }

    public void Play()
    {
        PresetActive();
        _gameStarted = false;
        _score = 0;
        ScoreText.text = _score.ToString();

        if (startScreenInstance != null)
        {
            Destroy(startScreenInstance);
            startScreenInstance = null;
        }

        Time.timeScale = 1f;
        Player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
        {
            _pool.ReturnBack(pipes[i].gameObject);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Player.enabled = false;
    }

    public void IncreaseScore()
    {
        _score++;
        ScoreText.text = _score.ToString();
    }

    public void UpdateHighScoreToUI()
    {
        HighScoreText.text = _highscore.ToString();
        HighScoreText.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        Table.SetActive(true);
        RestartGameButton.SetActive(true);
        FinalScoreText.gameObject.SetActive(true);
        GameOverText.SetActive(true);
        RenewButton.SetActive(true);
        
        if (_score > 5)
        {
            GoldMedal.SetActive(true);
        }
        else
        {
            SilverMeal.SetActive(true);
        }

        FinalScoreText.text = _score.ToString();
        _gameStarted = false;

        UpdateHighScoreToUI();
        Pause();
    }

    public void ResetHighScore()
    {
        _highscore = 0;
        PlayerPrefs.SetInt("HighScore", _highscore);
        PlayerPrefs.Save();

        UpdateHighScoreToUI();
        Play();
    }
}
