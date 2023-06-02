using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int coins { get; private set; }
    public int score { get; private set; }
    public int maxScore { get; private set; }

    private void Awake()
    {
        DataLoad();
        DataShow();
    }
    [SerializeField] Text cointText, scoreText, maxScoreText;

    public void CoinAdd(Coin coin)
    {
        coins += coin.sum;
        DataShow();
        DataSave();
    }
    public void ScoreAdd(Platform platform)
    {
        score += platform.score;

        if (maxScore < score)
        {
            maxScore = score;
        }

        DataShow();
        DataSave();
    }

    private void DataShow()
    {
        cointText.text = "Coins: "+coins.ToString();
        scoreText.text = "Score: " + score.ToString();
        maxScoreText.text = "Best Score: " + maxScore.ToString();
    }
    private void DataSave()
    {
        PlayerPrefs.SetInt("coint", coins);
        PlayerPrefs.SetInt("maxScore", maxScore);
    }
    private void DataLoad()
    {
        coins = PlayerPrefs.GetInt("coint", 0);
        maxScore = PlayerPrefs.GetInt("maxScore", 0);
    }
}
