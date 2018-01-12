using UnityEngine;
using UnityEngine.UI;

public class PointObject : MonoBehaviour
{
    [SerializeField] private int itemScore;
    [SerializeField] private GameObject scoreManager;
    [SerializeField] private GameObject airplain;

    public void OnTriggerEnter(Collider other) {
        scoreManager.GetComponent<ScoreManager>().AddScore(itemScore);
        Destroy(gameObject);
    }
    
    public void Update() {
        Vector3 airplainPosition = airplain.GetComponent<Transform>().position;
        Vector3 myPosition = GetComponent<Transform>().position;
        float distance = Vector3.Distance(airplainPosition, myPosition);
        Debug.Log("Distance : " + distance);
    }
}
