using UnityEngine;

public class ScoreManeger : MonoBehaviour
{
	private int score;

	public void Start()
	{
		score = 0;
	}

	public void AddScore(int plusScore)
	{
		score += plusScore;
		Debug.Log(score);
	}
}
