using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectDotManager : MonoBehaviour {
    [SerializeField] private GameObject playerDot;
    [SerializeField] private GameObject pointObjectDot;
    private List<GameObject> pointObjectSet = new List<GameObject>();
    private List<GameObject> pointObjectDotSet = new List<GameObject>();
    [SerializeField] private GameObject airplain;

    public void GenerateDot(List<GameObject> pointObjectSet) {
        this.pointObjectSet = pointObjectSet;
        foreach (var pointObject in pointObjectSet) {
            var pointObjectPosition = new Vector2 {
                x = pointObject.GetComponent<Transform>().position.x,
                y = pointObject.GetComponent<Transform>().position.z
            };
            var playerPosition = new Vector2 {
                x = airplain.GetComponent<Transform>().position.x,
                y = airplain.GetComponent<Transform>().position.z
            };
            var playerDotPosition = playerDot.GetComponent<Transform>().;
            pointObjectDotSet.Add(Instantiate(pointObjectDot, ));
        }
    }

    public void Update () {
        Vector3 airplainPosition = airplain.GetComponent<Transform>().position;
        Vector3 myPosition = GetComponent<Transform>().position;
        float distance = Vector3.Distance(airplainPosition, myPosition);
        Debug.Log("Distance : " + distance);
    }
}
