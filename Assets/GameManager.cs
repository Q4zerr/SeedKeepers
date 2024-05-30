using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    public float currentScore = 0f;
    public int lives = 3;
    public Data data;

    public bool isPlaying = false;

    public UnityEvent onPlay = new UnityEvent();

    public UnityEvent onGameOver = new UnityEvent();

    private void Start()
    {
        // If we have data load data 
        string loadedData = SaveSystem.Load("save");
        if (loadedData != null)
        {
            data = JsonUtility.FromJson<Data>(loadedData);
        }
        else
        {
            // else create new data
            data = new Data();
        }

        // Load the save seed score
        ScoreManager.Instance.LoadScore();
    }

    private void Update()
    {
        if (isPlaying)
        {
            float scoreMultiplier = CalculateScoreMultiplier(ScoreManager.Instance.score);
            currentScore += Time.deltaTime * scoreMultiplier;
        }
    }

    public void StartGame()
    {
        onPlay.Invoke();
        isPlaying = true;
        currentScore = 0;
        lives = 3;
    }

    public void GameOver()
    {
        if (data.highscore < currentScore)
        {
            data.highscore = currentScore;

            string saveString = JsonUtility.ToJson(data);

            SaveSystem.Save("save", saveString);
        }
        isPlaying = false;

        onGameOver.Invoke();

        // Save the seedScore when the game is over
        ScoreManager.Instance.SaveScore();
    }

    public string PrettyScore()
    {
        return Mathf.RoundToInt(currentScore).ToString();
    }

    public string PrettyHighScore()
    {
        return Mathf.RoundToInt(data.highscore).ToString();
    }

    public int GetScore()
    {
        return ScoreManager.Instance.score;
    }

    private float CalculateScoreMultiplier(int seedScore)
    {
        int multiplierSteps = seedScore / 10;
        float boost = 1.0f + multiplierSteps * 0.05f;
        return boost;
    }

    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            // Mettez à jour l'UI des vies
            UIManager.Instance.UpdateLives(lives);
        }
    }
}
