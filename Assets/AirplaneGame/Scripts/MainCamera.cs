using System;
using UnityEngine;

public class MainCamera : MonoBehaviour 
{
	//カメラのモードを決める為のスイッチ変数を宣言する
	public int _Mode = 0; //0=俯瞰,1=主観
	//カメラの相対位置、角度を宣言する
	//public float _CameraPx = 0f; //カメラの機体からの距離x
	public float _Rotation = 20f; //カメラの初期角度x
	private float _CameraRy = 0f; //カメラ初期角度y

	public float _Distance = 20f;
	public float _OffSet = 0.5f;
	//該当するオブジェクトとその特定コンポーネントへ関連付ける為の枠を作る。（機体を割り当てる）
	public Transform _AirPlane; //Transformは座標などを司るデフォルトのコンポーネント名。変更不可。
	
	//読み込んだデータを格納計算するための変数を宣言する
	//private Vector3 _lastRotation = Vector3.zero; //機体の前回の角度を格納する変数
	//private Vector3 _diffRotation = Vector3.zero; //機体の角度の変化量を格納する変数

	// Use this for initialization
	public void Start () 
	{/*
		_Mode = 0;
		//カメラが対象を俯瞰できる初期位置と角度を格納する
		//CameraPx = 0f;
		//CameraPy = 20f;
		//CameraPz = -15f;
		//CameraRx = 25f;
		//CameraRy = 0f;
		//起動時に機体の角度を取得しておく
		_LastRotation = BodyObject.rotation.eulerAngles;
		if (_LastRotation.x > 180)
			_LastRotation.x -= 360;
		if (_LastRotation.y > 180)
			_LastRotation.y -= 360;
		if (_LastRotation.z > 180)
			_LastRotation.z -= 360;
		//起動時に機体の座標を取得しておく
		_airPlanePosition = BodyObject.position;
		//カメラの位置を機体と同一にする
		GetComponent<Transform>().position = _airPlanePosition;

		Setup(_LastRotation);
		//決定された角度にする
		GetComponent<Transform>().rotation = Quaternion.Euler(_Rotation);				
		//決定された位置にカメラを置く
		GetComponent<Transform>().position = _CameraPosition;*/
	}

	private Vector3 GetRelativePosition(Vector3 rotation) //引数として現在の機体の角度を扱う
	{

		//俯瞰時のカメラの位置を決定する
		/* カメラ位置に関するメモ
		機体のx角が0(北)を向いていて(3,4,5)にいた場合、カメラは(3+(0*1), 4+3, 5+(-5*1))にいてy角0を向いている必要がある
		機体のx角が90(東)を向いていて(3,4,5)にいた場合、カメラは(3+(-5*1), 4+3, 5+(0*-1))にいてy角90を向いている必要がある
		機体のx角が180(南)を向いていて(3,4,5)にいた場合、カメラは(3+(0*-1), 4+3, 5+(-5*-1))にいてy角180を向いている必要がある
		機体のx角が-90(西)を向いていて(3,4,5)にいた場合、カメラは(3+(-5*-1), 4+3, 5+(0*1))にいてy角-90を向いている必要がある

		機体のx角が0(北)を向いていて(0,0,0)にいた場合、カメラは(0+(0*1), 0+3, 0+(-5*1))にいてy角0を向いている必要がある
		機体のx角が90(東)を向いていて(0,0,0)にいた場合、カメラは(0+(-5*1), 0+3, 0+(0*-1))にいてy角90を向いている必要がある
		機体のx角が180(南)を向いていて(0,0,0)にいた場合、カメラは(0+(0*-1), 0+3, 0+(-5*-1))にいてy角180を向いている必要がある
		機体のx角が-90(西)を向いていて(0,0,0)にいた場合、カメラは(0+(-5*-1), 0+3, 0+(0*1))にいてy角-90を向いている必要がある

		機体x角が0; カメラxは相対xの値が正の値で乗算され相対zは0、zは相対zの値が正の値で乗算され相対xは0になる
		機体x角が90; カメラxは相対zの値が正の値で乗算され相対xは0、zは相対xの値が負の値で乗算され相対zは0になる
		機体x角が180; カメラxは相対xの値が負の値で乗算され相対zは0、zは相対zの値が負の値で乗算され相対xは0になる
		機体x角が270; カメラxは相対zの値が負の値で乗算され相対xは0、zは相対xの値が正の値で乗算され相対zは0になる
		機体x角をθとしたとき、カメラxは「機体座標x + cos(θ-90)*相対z + sin(θ-90)*相対x」,カメラzは「機体座標z - cos(θ-90)*相対x - sin(θ-90)*相対z」で求める

		Unityの三角関数の角度の単位はラジアンなので、機体のx角をラジアンに変換する 円周率はMathf.PIを用いる
		調整後角度0 = ラジアン0 ; Cos0π = 1, Sin0π = 0
		調整後角度1 = ラジアンπ/180 ;
		調整後角度90 = ラジアンπ/2 ; Cosπ/2 = 0, Sinπ/2 = 1
		調整後角度180 = ラジアンπ ; Cosπ = -1, Sinπ = 0
		調整後角度270 = ラジアン-π/2 ; Cos-π/2 = 0, Sin-π/2 = -1
		調整後角度θ = ラジアンπθ/180;
		*/
		
		/* カメラ位置に関するメモ ver.2
		機体のy角が0(北)を向いていてy角が0（正面）を向いていて(3,4,5)にいた場合、カメラは(3+(0*1), 4+3, 5+(-5*1))にいてy角0を向いている必要がある
		機体のy角が0(北)を向いていてy角が90（上）を向いていて(3,4,5)にいた場合、カメラは(3+(0*1), 4+3, 5+(-5*1))にいてy角90を向いている必要がある
		
		機体のy角が0(東)を向いていて(3,4,5)にいた場合、カメラは(3+(-5*1), 4+3, 5+(0*-1))にいてy角0を向いている必要がある
		機体のy角が90(南)を向いていて(3,4,5)にいた場合、カメラは(3+(0*-1), 4+3, 5+(-5*-1))にいてy角90を向いている必要がある
		機体のy角が180(西)を向いていて(3,4,5)にいた場合、カメラは(3+(-5*-1), 4+3, 5+(0*1))にいてy角180を向いている必要がある

		機体のy角が0(北)を向いていて(0,0,0)にいた場合、カメラは(0+(0*1), 0+3, 0+(-5*1))にいてy角0を向いている必要がある
		機体のy角が90(東)を向いていて(0,0,0)にいた場合、カメラは(0+(-5*1), 0+3, 0+(0*-1))にいてy角90を向いている必要がある
		機体のy角が180(南)を向いていて(0,0,0)にいた場合、カメラは(0+(0*-1), 0+3, 0+(-5*-1))にいてy角180を向いている必要がある
		機体のy角が-90(西)を向いていて(0,0,0)にいた場合、カメラは(0+(-5*-1), 0+3, 0+(0*1))にいてy角-90を向いている必要がある

		機体y角が0; カメラxは相対xの値が正の値で乗算され相対zは0、zは相対zの値が正の値で乗算され相対xは0になる
		機体y角が90; カメラxは相対zの値が正の値で乗算され相対xは0、zは相対xの値が負の値で乗算され相対zは0になる
		機体y角が180; カメラxは相対xの値が負の値で乗算され相対zは0、zは相対zの値が負の値で乗算され相対xは0になる
		機体y角が270; カメラxは相対zの値が負の値で乗算され相対xは0、zは相対xの値が正の値で乗算され相対zは0になる
		機体y角をθとしたとき、カメラxは「機体座標x + cos(θ-90)*相対z + sin(θ-90)*相対x」,カメラzは「機体座標z - cos(θ-90)*相対x - sin(θ-90)*相対z」で求める
		*/
		
		//機体のy角を-179.9～180.0の範囲から0.0～359.9の範囲に直しつつ東向きを0度に規定
		var degY/*旧xAdjust*/ = rotation.y;
		var degX = rotation.x;
		/*if (degY >= 360f)
		{
			degY -= 360f;		// 360度内に収まらなくても三角関数はも止まるのでここはいらない
		}*/ 
		var radY/*旧xTheta*/ = degY * Mathf.Deg2Rad /*= Mathf.Abs(degY) * Mathf.PI / 180*/;
		var radX = degX * Mathf.Deg2Rad + _OffSet / 100;
		//Debug.Log("調整後機体x角;"+xAdjust+", xTheta=ラジアン換算;"+xTheta+", Cos(xTheta);"+Mathf.Cos(xTheta)+", Sin(xTheta);"+Mathf.Sin(xTheta));

		//俯瞰時のカメラと機体との相対座標を決定する
		var relativePosition = new Vector3(0, 0, 0)
		{
			x = -_Distance * Mathf.Sin(radY) * Mathf.Cos(-radX),
			y = _Distance * Mathf.Sin(radX),
			z = -_Distance * Mathf.Cos(-radX) * Mathf.Cos(-radY)
		};
		//x = (Mathf.Cos(radY) * _CameraPz + Mathf.Sin(radY) * _CameraPx) - (Mathf.Cos((-rotation.x - _Rotation)* Mathf.Deg2Rad) * _CameraPx),
		//相対的な機体との距離設定を取得 ここではy値のみ取得
		//Debug.Log("相対カメラ位置;"+relativePosition);
		
		//相対座標を返す
		return relativePosition; 
	}
	
	// Update is called once per frame
	public void Update () 
	{
		//カメラをBodyの子オブジェクトにする
		//this.transform.parent = BodyObject;
		//機体の現在の角度を取得する
		var nowRotation = _AirPlane.rotation.eulerAngles;
		if (nowRotation.x > 180)
		{
			nowRotation.x -= 360;
		}
		if (nowRotation.y > 180)
		{
			nowRotation.y -= 360;
		}
		if (nowRotation.z > 180)
		{
			nowRotation.z -= 360;
		}
		//Debug.Log("現在機体角度;"+NowRotation);
		//俯瞰時のカメラの角度を決定する
		var cameraRotation = new Vector3(_Rotation + nowRotation.x, _CameraRy + nowRotation.y, 0f); //絶対的な角度設定を取得

		//Debug.Log("カメラ現在位置"+GetComponent<Transform>().position);

		//カメラの角度と座標を決める
		switch (_Mode)
		{
		case 0: //俯瞰
			//決定された位置にカメラを置く
			GetComponent<Transform>().position = _AirPlane.position + GetRelativePosition(nowRotation);
			//Setup(_NowRotation);
			//Setup()で決定されたカメラの角度と座標へカメラを移動させる

			//カメラの角度を決定
			//_diffRotation = _lastRotation - nowRotation; //Bodyの前回の角度と現在の角度の差を求める
			//Debug.Log("機体角度変化量;"+_diffRotation);
			//Bodyの角度変化に合わせてCameraの角度を調整 Bodyはxの値が-で上方へ、yの値が-で左へ行く
			//cameraRotation.x += _diffRotation.x * -0.25f;
			

			//決定された角度にする
			GetComponent<Transform>().rotation = Quaternion.Euler(cameraRotation);
			//Debug.Log("カメラ角度"+CameraRotation);
			//Debug.Log("カメラ最終位置"+CameraPosition);			
			break;

		/*case 1: //主観
			//Bodyの現在の角度に合わせてCameraの角度を変更
			CameraRotation.x = _NowRotation.x * 1f;
			CameraRotation.y = _NowRotation.y * 1f;
			CameraRotation.z = _NowRotation.z * 1f;
			//カメラの角度を決定
			GetComponent<Transform>().rotation = Quaternion.Euler(CameraRotation);
			
			//カメラの位置を決定する
			_CameraPosition.x = _airPlanePosition.x + _CameraPx * 0f;
			_CameraPosition.y = _airPlanePosition.y + _CameraPy * 0.5f;
			_CameraPosition.z = _airPlanePosition.z + _CameraPz * 0f;
			//決定された位置にカメラを置く
			GetComponent<Transform>().position = _CameraPosition;
			break;

		default: //上記以外
			_Mode = 0;
			Setup(_NowRotation);
			
			//決定された角度にする
			GetComponent<Transform>().rotation = Quaternion.Euler(CameraRotation);
			//決定された位置にカメラを置く
			GetComponent<Transform>().position = _CameraPosition;
			break; //Unityではswitch文でbreakしかジャンプ文が使えない？*/
		}
		
		//次回に持ち越すため今回の機体の角度を格納
		//_lastRotation = nowRotation;
	}

}