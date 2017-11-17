using UnityEngine;
using UnityEngine.UI;

public class HUDText : MonoBehaviour {
	[SerializeField] private Text throtolText;
	[SerializeField] private Text scoreText;

	private void Start ()
	{
		throtolText = GetComponent<Text> ();
	}

	public void UpdateScoreText(int score)
	{
		scoreText.text = "Score:" + score + "P";
	}

	public void UpdateThrotolText()
	{
		throtolText.text = "スロットル:";
	}
}
