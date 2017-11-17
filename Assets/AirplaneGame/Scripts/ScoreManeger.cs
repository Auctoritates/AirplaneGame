using UnityEngine;

public class ScoreManeger : MonoBehaviour
{
	private static int score;

	public static void AddScore(int plusScore)
	{
		Debug.Log("test");
		score += plusScore;
	}
}
