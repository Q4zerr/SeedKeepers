using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int score { get; private set; }
    public UnityEvent<int> onScoreChanged = new UnityEvent<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        onScoreChanged.Invoke(score);
    }

    public void SaveScore()
    {
        GameManager.Instance.data.seedScore = score;
        string saveString = JsonUtility.ToJson(GameManager.Instance.data);
        SaveSystem.Save("save", saveString);
    }

    public void LoadScore()
    {
        score = (int)GameManager.Instance.data.seedScore;
        onScoreChanged.Invoke(score);
    }
}
