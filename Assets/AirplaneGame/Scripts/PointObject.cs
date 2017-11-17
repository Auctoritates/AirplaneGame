using UnityEngine;

public class PointObject : MonoBehaviour
{
	[SerializeField] private int itemScore;
	[SerializeField] private GameObject scoreManager;

	public void OnTriggerEnter(Collider other) {
		scoreManager.GetComponent<ScoreManager>().AddScore(itemScore);
		Destroy(gameObject);
	}
}
