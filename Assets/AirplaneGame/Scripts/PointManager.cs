using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour {

	public GameObject pointObject;
	public int pointObjectNum = 10;
	public float areaX = 0, areaY = 0, areaZ = 0;
	public GameObject pointObjectDotManager;
	private readonly List<GameObject> pointObjectSet = new List<GameObject>();

	// Use this for initialization
	void Start () {
		for (var i = 0; i < pointObjectNum; i++) {
			var randomPosition = new Vector3 {
				x = Random.Range(0.0f, areaX),
				y = Random.Range(0.0f, areaY),
				z = Random.Range(0.0f, areaZ)
			};

			pointObjectSet.Add(Instantiate(pointObject, randomPosition, Quaternion.identity));
			pointObjectDotManager.GetComponent<PointObjectDotManager>().GenerateDot(pointObjectSet);
		}

	}

}
