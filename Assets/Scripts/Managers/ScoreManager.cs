using CoreFeatures.MessageBus;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text Score;
    public Text BestScore;

    private int _score = 0;
    //--------------------------------------------------------------------------

    private void Awake()
    {
        MessageBus.GetInstance().Subsribe("TowerDestructed", OnTowerDectructed);
    }
    //--------------------------------------------------------------------------

    public void OnTowerDectructed(Message message)
    {
        ++_score;
        Score.text = _score.ToString();
    }
    //--------------------------------------------------------------------------

    public void OnGameOver()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (bestScore < _score)
        {
            bestScore = _score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        BestScore.text += bestScore.ToString();

        BestScore.gameObject.SetActive(true);
    }
    //--------------------------------------------------------------------------
}
