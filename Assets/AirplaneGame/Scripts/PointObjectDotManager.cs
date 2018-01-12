using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectDotManager : MonoBehaviour {
    [SerializeField] private GameObject airplain;
    [SerializeField] private GameObject airplainDot;
    [SerializeField] private GameObject pointObjectDot;
    [SerializeField] private GameObject panel;
    private List<GameObject> pointObjectSet = new List<GameObject>();
    private readonly List<GameObject> pointObjectDotSet = new List<GameObject>();

    public void GenerateDot(List<GameObject> pointObjectSet) {
        this.pointObjectSet = pointObjectSet;
        foreach (var pointObject in pointObjectSet) {
            var distance = getDistance(pointObject);
            var instantiatedPointObjectDot = Instantiate(pointObjectDot, distance, Quaternion.identity);
            instantiatedPointObjectDot.transform.SetParent(panel.transform, false);
            pointObjectDotSet.Add(instantiatedPointObjectDot);
        }
    }

    public void Update () {
        for(var i = 0; i < pointObjectSet.Count; i++) {
            var distance = getDistance(pointObjectSet[i]);
            var airplainDotPosition = airplainDot.GetComponent<Transform>().position;
            var pointObjectDotPosition = new Vector2() {
                x = airplainDotPosition.x + distance.x,
                y = airplainDotPosition.y + distance.y
            };
            pointObjectDotSet[i].GetComponent<Transform>().position = pointObjectDotPosition;
        }
    }

    private Vector2 getDistance(GameObject pointObject) {
        var airplainDotPosition = airplainDot.GetComponent<Transform>().position;
        var pointObjectPosition = new Vector2 {
            x = pointObject.GetComponent<Transform>().position.x,
            y = pointObject.GetComponent<Transform>().position.z
        };
        var airplainPosition = new Vector2 {
            x = airplain.GetComponent<Transform>().position.x,
            y = airplain.GetComponent<Transform>().position.z
        };
        var distance = new Vector2() {
            x = (pointObjectPosition.x - airplainPosition.x) / 20,
            y = (pointObjectPosition.y - airplainPosition.y) / 20
        };
        return distance;
    } 
}
