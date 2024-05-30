using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI seedScoreUI;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverScoreUI;
    [SerializeField] private TextMeshProUGUI gameOverHighscoreUI;
    [SerializeField] private GameObject lifeContainer;
    [SerializeField] private GameObject heartPrefab;

    GameManager gm;
    private List<GameObject> hearts = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gm = GameManager.Instance;
        gm.onGameOver.AddListener(ActivateGameOverUI);
        ScoreManager.Instance.onScoreChanged.AddListener(UpdateScoreUI);

        // Update the UI with the loaded seed score
        UpdateScoreUI(ScoreManager.Instance.score);

        // Initialize the hearts UI
        InitializeHearts(gm.lives);
    }

    public void PlayButtonHandler()
    {
        gm.StartGame();
        startMenuUI.SetActive(false);
        InitializeHearts(gm.lives);
    }

    public void ActivateGameOverUI()
    {
        gameOverUI.SetActive(true);

        gameOverScoreUI.text = "Score : " + gm.PrettyScore();
        gameOverHighscoreUI.text = "Highscore : " + gm.PrettyHighScore();
    }

    private void OnGUI()
    {
        scoreUI.text = gm.PrettyScore();
    }

    private void UpdateScoreUI(int newScore)
    {
        seedScoreUI.text = newScore.ToString();
    }

    public void InitializeHearts(int numberOfHearts)
    {
        // Clear previous hearts
        foreach (Transform child in lifeContainer.transform)
        {
            child.gameObject.SetActive(false);
        }

        // Create new hearts if needed
        for (int i = hearts.Count; i < numberOfHearts; i++)
        {
            GameObject heart = Instantiate(heartPrefab, lifeContainer.transform);
            hearts.Add(heart);
        }

        // Activate hearts based on current lives
        for (int i = 0; i < numberOfHearts; i++)
        {
            hearts[i].SetActive(true);
        }
    }

    public void UpdateLives(int currentLives)
    {
        // Update heart images
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < currentLives);
        }
    }
}
