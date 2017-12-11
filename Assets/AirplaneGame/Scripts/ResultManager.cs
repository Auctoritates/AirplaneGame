using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour 
{
	[SerializeField] private GameObject _ResultUI;
	public string _ActionCode;
	public bool _IsResultOK;

	// Use this for initialization
	void Start () 
	{
		SetDefault();
		SetResultOff();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_IsResultOK)
		{
			SetResultOn();
		}
		Action();
		if (_ActionCode == "End")
		{
			SetResultOff();
		}
	}
	void SetDefault()
	{
		_ActionCode = "";
		_IsResultOK = false;
	}
	void SetResultOn()
	{
		_ResultUI.SetActive(true);
	}
	void SetResultOff()
	{
		_ResultUI.SetActive(false);				
	}
	void Action()
	{
		if (Input.GetButton("Submit"))
		{
			_ActionCode = "End";
		}
	}
}