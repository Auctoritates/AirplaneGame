using UnityEngine;

public class PointObject : MonoBehaviour
{
	[SerializeField] private int itemScore;
	private ScoreManeger _scoreManeger;

	public void OnTriggerEnter(Collider other)
	{
		_scoreManeger = GetComponent<ScoreManeger>();
		_scoreManeger.AddScore(itemScore);
		Destroy(gameObject);
	}
}
