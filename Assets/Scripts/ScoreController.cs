using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
	public Text Score;
	public Text BestScore;

	private int _score = 0;
	//--------------------------------------------------------------------------

	public void OnTowerDectructed()
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
