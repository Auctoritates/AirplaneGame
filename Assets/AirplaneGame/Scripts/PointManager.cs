using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour {

	public GameObject pointObject;
	public int pointObjectNum = 10;
	public float areaX = 0, areaY = 0, areaZ = 0;

	// Use this for initialization
	void Start () {

		for (int i = 0; i < pointObjectNum; i++) {
			float x = Random.Range (0.0f, areaX);
			float y = Random.Range (0.0f, areaY);
			float z = Random.Range (0.0f, areaZ);

			Instantiate (pointObject, new Vector3 (x, y, z), Quaternion.identity);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
