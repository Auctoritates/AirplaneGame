using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour 
{
	[SerializeField] private int _GoalMode;
	[SerializeField] private GameObject _BodyObject; //機体オブジェクトを登録する場所を作る
	[SerializeField] private GameObject _GoalObject; //ゴールオブジェクトを登録する場所を作る
	[SerializeField] private GameObject _GoalUI; //ゴール時に表示するエフェクトのUIを登録する場所を作る
	[SerializeField] private Camera _MainCamera;
	[SerializeField] private Camera _CameraForResult;
	private GameObject _ResultManager;
	ResultManager _ResultManagerScript;

	[SerializeField] private Image _GBImage; //クリア時の演出イメージ背景黒
	[SerializeField] private Image _GWImage; //クリア時の演出イメージ背景白
	[SerializeField] private Text _GText; //クリア時の描画メッセージテキスト
	private Image _TmpImage;
	private Text _TmpText;
	private Vector3 _BodyPosition = Vector3.zero; //機体の現在の位置を格納する変数
	private Vector3 _GoalPosition = Vector3.zero; //ゴールの現在の位置を格納する変数
	private Vector3 _GoalScale = Vector3.zero; //ゴールの現在の大きさを格納する変数
	private Color _GBImageColor = Vector4.zero;
	private Color _GWImageColor = Vector4.zero;
	private Color _GTColor = Vector4.zero;
	private Color _TmpColor = Vector4.zero;
	private int _WaitCount;
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
		_WaitCount = 0;
		
		_ResultManager = GameObject.Find("ResultManager");
		_ResultManagerScript = _ResultManager.GetComponent<ResultManager>();
		_MainCamera = Camera.main;
		_MainCamera.GetComponent<Camera>().enabled = true;
		_CameraForResult.GetComponent<Camera>().enabled = false;
		_GoalMode = 0;

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
		if (_GoalMode == 0)
		{
			if (_GoalMaxX > _BodyPosition.x && _BodyPosition.x > _GoalMinX)
			{
				//Debug.Log("X範囲内到達");
				if (_GoalMaxZ > _BodyPosition.z && _BodyPosition.z > _GoalMinZ)
				{
					//Debug.Log("Z範囲内到達");
					Goal();
				}
			}
		}
		else
		{
			//既にゴールしているなら
			Goal();
		}
	}
	void AppearImage(Image _TmpImage)
	{
		_TmpColor = _TmpImage.GetComponent<Image>().color;
		_TmpColor.a = _Alpha;
		_TmpImage.GetComponent<Image>().color = _TmpColor;		
	}
	void AppearText(Text _TmpText)
	{
		_TmpColor = _TmpText.GetComponent<Text>().color;
		_TmpColor.a = _Alpha;
		_TmpText.GetComponent<Text>().color = _TmpColor;
	}
	
	void GoalEffect1()//時間をスローにする
	{
		//Debug.Log("Clear!!!!!!!!!!!!!!!!!!!!!!!!!!");
		Time.timeScale = 0.2f;
		_GoalUI.SetActive(true);	
	}
	void GoalEffect2()//段々とゴールUIを表示する
	{
		AppearImage(_GBImage);
		AppearImage(_GWImage);
		AppearText(_GText);
	}

	void EffectOff()
	{
		Debug.Log("ゴール画面終了");
		_GoalUI.SetActive(false);
		_ResultManagerScript._IsResultOK = true;
	}
	
	void Goal()
	{
		switch (_GoalMode)
		{
			case 0://ゴール直後
				_GoalMode++;
				break;
			case 1://ゴール後
				if (_WaitCount == 0)
				{
					_Alpha = 0f;
					GoalEffect1();
					GoalEffect2();
					_WaitCount++;
				}
				else if (_WaitCount > 255)
				{
					Debug.Log("スロー終了");
					_GoalMode++;
				}
				else if (_Alpha < 1f)
				{
					Debug.Log("追加");
					_Alpha += 5f/255f;
					GoalEffect2();
					_WaitCount++;	
				}
				else
				{
					Debug.Log("表示終了");
					_WaitCount++;
				}
				break;
			case 2://ゴール後エフェクト描画済み
				if (Input.GetButton("Submit"))
				{	
					EffectOff();
					_GoalMode++;
					_WaitCount = 0;
					_ResultManagerScript._IsResultOK = true;
					_MainCamera.GetComponent<Camera>().enabled = false;
					_CameraForResult.GetComponent<Camera>().enabled = true;
				}
				break;
			case 3://ゴール後エフェクト撤去済み
				//リザルト画面を描画する時間を稼ぐ
				_WaitCount++;
				if (_WaitCount > 60)
				{
					_GoalMode++;
				}
				break;
			case 4:
					Time.timeScale = 1.0f;
					_ResultManagerScript._IsEndOK = true;
					break;				
			default:
				break;
		}
	}
}
