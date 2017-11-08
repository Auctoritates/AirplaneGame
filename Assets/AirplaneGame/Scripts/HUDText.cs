using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDText : MonoBehaviour {

	public Text throtol;
	// Use this for initialization
	void Start () {
		//throtol = this.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		throtol.text = "スロットル:";
	}
}
