using UnityEngine;
using UnityEngine.UI;

public class HUDText : MonoBehaviour {
    [SerializeField] private GameObject airplane;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text heightText;
    
    public void UpdateScoreText(int score) {
        scoreText.text = "Score:" + score + "P";
    }

    public void Update() {
        heightText.text = "Height:" + (airplane.GetComponent<Transform>().position.y / 100 + 5000) + "km";
    }
}
