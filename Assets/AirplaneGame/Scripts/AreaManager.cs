using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour {

	//場外判定用の変数
	[SerializeField] private GameObject _Airplane;
	private Vector3 _PositionNow;
	private Vector3 _AbsolutePosNow;
	private Vector3 _DefaultPos;
	private Vector3 _Range;
	private Vector3 _Alert;
	private float _MagnificationAlert;
	private int _RangeState;//0=エリア内, 1=アラート圏内, 2=エリア逸脱

	//エフェクト用の変数
	//[SerializeField] private Image _Alert1;
	private Color _WarningColor;
	private Color _Clear;

	// Use this for initialization
	void Start () 
	{
		SetDefault();
	}
	
	// Update is called once per frame
	void Update () 
	{
		OutDetect();

		if (_RangeState == 1)
		{
			Alert();
		}
		else if (_RangeState == 2)
		{
			AreaOver();
		}			

	}
	
	void SetDefault() //初期値設定
	{
		_DefaultPos = _Airplane.transform.position;

		_Range = new Vector3(20000f, 20000f, 20000f);
		_MagnificationAlert = 0.8f;
		_Alert = new Vector3(_Range.x * _MagnificationAlert, _Range.y * _MagnificationAlert, _Range.z * _MagnificationAlert);
				
		_Range = new Vector3(Math.Abs(_Range.x), Math.Abs(_Range.y), Math.Abs(_Range.z)); 
		_Alert = new Vector3(Math.Abs(_Alert.x), Math.Abs(_Alert.y), Math.Abs(_Alert.z));
		
		_RangeState = 0;
		
		_Clear = Color.clear;
		_WarningColor = new Vector4(1f, 0.4f, 0.4f);
	}

	void OutDetect() //場外逸脱を検出
	{
		_PositionNow = _Airplane.transform.position;
		_AbsolutePosNow.x = Math.Abs(_PositionNow.x);
		_AbsolutePosNow.y = Math.Abs(_PositionNow.y);
		_AbsolutePosNow.z = Math.Abs(_PositionNow.z);

		_RangeState = 0;
		if (_AbsolutePosNow.x > _Range.x || _AbsolutePosNow.y > _Range.y || _AbsolutePosNow.z > _Range.z)
		{
			_RangeState = 2;
		}
		else if (_AbsolutePosNow.x > _Alert.x || _AbsolutePosNow.y > _Alert.y || _AbsolutePosNow.z > _Alert.z)
		{
			_RangeState = 1;			
		}		
	
	}
	
	void AreaOver ()//逸脱時の処理
	{
		Debug.Log("逸脱");
		_Airplane.transform.position = _DefaultPos;
	}

	void Alert ()
	{
		Debug.Log("警告");		
	}
}
