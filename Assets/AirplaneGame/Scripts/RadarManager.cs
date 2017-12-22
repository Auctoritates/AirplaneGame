using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarManager : MonoBehaviour {
	[SerializeField] private GameObject airplane;
	
	// Update is called once per frame
	void Update () {
		Vector3 test = airplane.GetComponent<Transform>().position;
		GetComponent<Transform>().position += airplane.GetComponent<Transform>().position;

	}
}
