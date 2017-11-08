using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour 
{
	public string _ModeSelectScene = "";
	public Canvas _OptionCanvas;//子オブジェクトのキャンバス プレハブ化しても設定が活きる

	// Use this for initialization
	void Start () 
	{
		//ModeSelectシーンを取得する
		_ModeSelectScene = "ModeSelect";

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButton("Cancel"))
		{
			//決定キー入力を受け付ける
			//ModeSelectシーンへ戻る
			Debug.Log("戻る");
			SceneManager.LoadScene(_ModeSelectScene);
		}
	}
}
