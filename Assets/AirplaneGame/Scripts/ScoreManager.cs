using UnityEngine;

public class ScoreManager : MonoBehaviour {
	private int score;
	[SerializeField] private GameObject hudManager;

	public void AddScore(int plusScore) {
		score += plusScore;
		hudManager.GetComponent<HUDText>().UpdateScoreText(score);
	}
}