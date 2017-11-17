using UnityEngine;

public class PointObject : MonoBehaviour
{
	[SerializeField] private int itemScore;

	public void OnTriggerEnter(Collider other)
	{
		ScoreManeger.AddScore(itemScore);
		Destroy(gameObject);
	}
}
