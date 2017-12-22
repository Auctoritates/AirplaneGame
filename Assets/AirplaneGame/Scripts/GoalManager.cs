using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour 
{
	//public bool _GoalMode = false; //
	ResultManager _ResultManagerScript;
	public GameObject _BodyObject; //機体オブジェクトを登録する場所を作る
	public GameObject _GoalObject; //ゴールオブジェクトを登録する場所を作る
	public GameObject _GoalUI; //ゴール時に表示するエフェクトのUIを登録する場所を作る
	public Image _GBImage; //クリア時の演出イメージ背景黒
	public Image _GWImage; //クリア時の演出イメージ背景白
	public Text _GText; //クリア時の描画メッセージテキスト
	private Vector3 _BodyPosition = Vector3.zero; //機体の現在の位置を格納する変数
	private Vector3 _GoalPosition = Vector3.zero; //ゴールの現在の位置を格納する変数
	private Vector3 _GoalScale = Vector3.zero; //ゴールの現在の大きさを格納する変数
	private Color _GBImageColor = Vector4.zero;
	private Color _GWImageColor = Vector4.zero;
	private Color _GTColor = Vector4.zero;
	private float _PlaneBaseSizeX = 0f;
	private float _PlaneBaseSizeZ = 0f;	
	private float _GoalMinX = 0f;
	private float _GoalMaxX = 0f;
	private float _GoalMinZ = 0f;
	private float _GoalMaxZ = 0f;
	private float _Alpha = 0f; //UIオブジェクトの透明度(0f～255f)
	
	// Use this for initialization
	void Start () 
	{
		//デバッグモードをオフにする
		//_DebugMode = false; 

		//ゴールUIの透明度を戻したうえで非アクティブにする
		_Alpha = 255f;
		_GBImageColor = _GBImage.GetComponent<Image>().color;
		_GBImageColor.a = _Alpha;
		_GBImage.GetComponent<Image>().color = _GBImageColor;
		_GWImageColor = _GWImage.GetComponent<Image>().color;
		_GWImageColor.a = _Alpha;
		_GWImage.GetComponent<Image>().color = _GWImageColor;
		_GTColor = _GText.GetComponent<Text>().color;
		_GTColor.a = _Alpha;
		_GText.GetComponent<Text>().color = _GTColor;
		
		//_WhiteImage.GetComponent<Image>().color.a = _Alpha;
		//_GoalText.GetComponent<Text>().color.a = _Alpha;
		_GoalUI.SetActive(false);

		//基準となるPlaneの基本サイズを設定する
		_PlaneBaseSizeX = 10f;
		_PlaneBaseSizeZ = 10f;
		
		//起動時にゴールオブジェクトの座標とスケールを取得しておく
		_GoalPosition = _GoalObject.GetComponent<Transform>().position;
		_GoalScale = _GoalObject.GetComponent<Transform>().lossyScale;
		//ゴールオブジェクトの範囲を計算する
		_GoalMinX = _GoalPosition.x - _PlaneBaseSizeX * _GoalScale.x /2;
		_GoalMaxX = _GoalPosition.x + _PlaneBaseSizeX * _GoalScale.x /2;
		_GoalMinZ = _GoalPosition.z - _PlaneBaseSizeZ * _GoalScale.z /2;
		_GoalMaxZ = _GoalPosition.z + _PlaneBaseSizeZ * _GoalScale.z /2;
	}
	
	// Update is called once per frame
	void Update () 
	{
		_BodyPosition = _BodyObject.GetComponent<Transform>().position;

		//デバッグ文
		//Debug.Log("機体位置"+_BodyPosition);		
		//Debug.Log("ゴール位置"+_GoalPosition);
		//Debug.Log("ゴールサイズX"+_GoalScale.x);
		//Debug.Log("ゴール範囲X："+_GoalMinX+"～"+_GoalMaxX);
		//Debug.Log("ゴール範囲Z："+_GoalMinZ+"～"+_GoalMaxZ);

		//機体がゴールオブジェクトの上空にいた場合
		if (_GoalMaxX > _BodyPosition.x && _BodyPosition.x > _GoalMinX)
		{
			//Debug.Log("X範囲内到達");
			if (_GoalMaxZ > _BodyPosition.z && _BodyPosition.z > _GoalMinZ)
			{
				//Debug.Log("Z範囲内到達");
				GoalEffect();
				if (Input.GetButton("Submit"))
				{
					EffectOff();
				}
			}
		}
	}
	
	void GoalEffect()
	{
		//Debug.Log("Clear!!!!!!!!!!!!!!!!!!!!!!!!!!");
		Time.timeScale = 0.5f;
		_GoalUI.SetActive(true);		
	}

	void EffectOff()
	{
		_GoalUI.SetActive(false);
		_ResultManagerScript._IsResultOK = true;
	}
}
