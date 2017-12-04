using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour {

	[SerializeField] private GameObject _Airplane;
	private float _RangeXmin;
	private float _RangeXmax;
	private float _RangeYmin;
	private float _RangeYmax;
	private float _RangeZmin;
	private float _RangeZmax;
	private float _AlertX;
	private float _AlertY;
	private float _AlertZ;

	private int _RangeState;//0=エリア内, 1=アラート圏内, 2=エリア逸脱

	
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
			Debug.Log("エリア逸脱");
		}
		else if (_RangeState == 2)
		{
			Debug.Log("エリア逸脱警告範囲内");	
		}
		
	}
	
	void SetDefault() //初期値設定
	{
		_RangeXmin = -50000f;
		_RangeXmax = _RangeXmin * -1;
		_RangeYmin = -50000f;
		_RangeYmax = _RangeYmin * -1;
		_RangeZmin = -50000f;
		_RangeZmax = _RangeZmin * -1;
		
		_AlertX = _RangeXmax * 0.8f;
		_AlertY = _RangeYmax * 0.8f;
		_AlertZ = _RangeZmax * 0.8f;
		
		
		_RangeState = 0;
	}

	void OutDetect()
	{
		//機体の座標取得
		//座標の位置判定
		
		_RangeState = 0;
	}
}
