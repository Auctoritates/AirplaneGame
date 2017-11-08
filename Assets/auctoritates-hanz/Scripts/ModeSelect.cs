using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeSelect : MonoBehaviour 
{
	//変数を宣言
	//モードセレクト画面のオブジェクト用の変数
	public string _TitleName = "";//このゲームのタイトル名
	public string _SceneGame = "";//ゲームシーンのファイル名
	private string _OptionMode = "";//オプション用の合図
	private string _End = "";//ゲーム終了用の合図
	private GameObject _OptionObject;//オプションオブジェクト
	private GameObject _StartButton;//スタートボタンオブジェクト
	private GameObject _OptionButton;//オプションボタンオブジェクト
	private GameObject _EndButton;//終了ボタンオブジェクト
	private Text _TitleText;//タイトルテキスト(最前)
	//カーソル移動に関する変数
	private GameObject _ButtonSelected;//現在選択されているボタンのオブジェクト
	private Color _Bright = Color.clear;//選択されているボタン用の明るい色
	private Color _Pale = Color.clear;//選択されていないボタン用の透明度の低い色
	private Color _Pressed = Color.clear;//決定されたボタン用のやや暗い色
	private int _SelectedNumber = 0;//現在選択されているボタンのID 0=スタートボタン, 1=オプションボタン, 2=終了ボタン
	private int _TimeAfterSelect = 0;//前回のカーソル移動後の経過時間
	private int _Interval = 0;//カーソル移動後、再びカーソル移動を受け付けるまでの時間設定
	private bool _IsSelectedNow = false;//true=カーソル移動が行われて間もない
	//一時的な値を格納する変数
	private string _SceneName = "";//読み込むシーンの名前
	private GameObject _TmpObject;//一時的に指定するゲームオブジェクト
	private Button _PointingButton;//指定したボタンオブジェクトのボタンコンポーネントを格納する変数
	private ColorBlock _TmpColorBlock;//一時的なボタンオブジェクトのボタンコンポーネントのカラーブロック
	private Image _TmpImage;//一時的なオブジェクトのイメージコンポーネント

	// Use this for initialization
	void Start () 
	{
		//Optionオブジェクトを非アクティブ化する
		_OptionObject = GameObject.Find("Option");
		_OptionObject.SetActive(false);

		//初期化
		_TitleName = "フライト";//このゲームのタイトル名を設定
		_SceneGame = "Scene1";//ゲーム用のシーンのファイル名(拡張子抜き)
		_OptionMode = "Option";//オプションオブジェクトの名前 またオプションの合図となる言葉
		_End = "End";//ゲーム終了用の合図となる言葉

		//UIオブジェクトを取得
		//キャンバス内のUIパーツの詳細はコンポーネント扱い
		_StartButton = GameObject.Find("StartButton");//スタートボタンのオブジェクトを取得
		_OptionButton = GameObject.Find("OptionButton");
		_EndButton = GameObject.Find("EndButton");

		_TmpObject = GameObject.Find("TitleText");
		_TitleText = _TmpObject.GetComponent<Text>();//タイトルテキストオブジェクトのテキストコンポーネントを取得
		_TitleText.text = _TitleName;//.textは小文字限定という初見お断り仕様
		
		//カーソル移動に関する変数を初期化
		_SelectedNumber = 0;//現在選択中のボタンをスタートボタンに設定
		_ButtonSelected = _StartButton;
		_TimeAfterSelect = 0;
		_Interval = 30 + 1;//目的の値に+1する
		_IsSelectedNow = false;
		_Bright = Color.white;//全ての値が1f(FF)、つまりFFFFFFFF、はっきりした白色
		_Pale = new Color(0.5f, 1f, 0.5f, 0.5f);//透明度のみ0.5f、薄い白色
		_Pressed = new Color(0.8f, 0.8f, 0.8f, 1f);//やや暗い白
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		//現在選択されているボタンに応じてボタンのカラーを変更する
		OffButton(_StartButton);
		OffButton(_OptionButton);
		OffButton(_EndButton);

		//選択IDから現在選択中のボタンを決定する
		switch(_SelectedNumber)
		{
			case 0:
				_ButtonSelected = _StartButton;
				_SceneName = _SceneGame;
				break;
				
			case 1:
				_ButtonSelected = _OptionButton;
				_SceneName = _OptionMode;
				break;
				
			case 2:
				_ButtonSelected = _EndButton;
				_SceneName = _End;//ゲーム終了の合図
				break;
				
			default:
				_SelectedNumber = 0;
				_ButtonSelected = _StartButton;
				_SceneName = _SceneGame;
				break;
		}
		OnButton(_ButtonSelected);

		//いずれかのボタンがクリックされていないか調べる
		
		
		//決定キー入力を受け付ける
		if(Input.GetButton("Submit"))
		{
			//対象のボタンの色を変更する
			ClickButton(_ButtonSelected);
			//決定かクリックされたボタンに応じて処理を行う
			Action();
		}

		//前回カーソル移動が成されていた場合そこから規定フレーム経っているか判定
		if (_IsSelectedNow)
		{
			if (_TimeAfterSelect >= _Interval)
			{
				_IsSelectedNow = false;
				_TimeAfterSelect = 0;
			}
			else
			{
				_TimeAfterSelect += 1;
			}
		}
		//カーソル移動の判定
		if (_TimeAfterSelect == 0)
		{
			//選択キー入力の結果に応じて選択中のボタンIDを変更する
			if (Input.GetAxis("Vertical") > 0)
			{
				//上
				//Debug.Log("上:Vertical < 0");
				_SelectedNumber += -1;
				_IsSelectedNow = true;
				_TimeAfterSelect = 1;
			}
			else if (Input.GetAxis("Vertical") < 0)
			{
				//下
				_SelectedNumber += 1;
				_IsSelectedNow = true;
				_TimeAfterSelect = 1;
			}

			//選択中のボタンIDの上限と下限を設け、超えていた場合逆の閾値にする
			if (_SelectedNumber < 0)
			{
				_SelectedNumber = 2;
			}
			else if (_SelectedNumber > 2)
			{
				_SelectedNumber = 0;
			}
		}
	}
	
	//指定されたゲームオブジェクトのボタンコンポーネント及びイメージコンポーネントを明るいカラーにする
	/*
		カラーはカラーブロックの代入で編集できる
		カラーはImageコンポーネントのcolorとButtonコンポーネントのnormalColorの2項目の乗算で描かれている
	*/
	void OnButton (GameObject _TmpObject)
	{
		//Debug.Log("OnButton");
		_PointingButton = _TmpObject.GetComponent<Button>();
		_TmpColorBlock = _PointingButton.colors;//指定されたボタンコンポーネントのカラーブロックを取得
		_TmpColorBlock.normalColor = _Bright;//カラーブロック内の平常時の色を_Brightにする
		_TmpColorBlock.highlightedColor = _Bright;
		_TmpColorBlock.pressedColor = _Bright;
		_PointingButton.colors = _TmpColorBlock;//カラーブロックを代入

		_TmpImage = _PointingButton.GetComponent<Image>();
		_TmpImage.color = _Bright;
	}

	//指定されていないゲームオブジェクトのボタンコンポーネント及びイメージコンポーネントを薄いカラーにする
	void OffButton(GameObject _TmpObject)
	{
		_PointingButton = _TmpObject.GetComponent<Button>();
		_TmpColorBlock = _PointingButton.colors;//指定されたボタンコンポーネントのカラーブロックを取得
		_TmpColorBlock.normalColor = _Pale;
		_TmpColorBlock.highlightedColor = _Pale;
		_TmpColorBlock.pressedColor = _Pale;
		_PointingButton.colors = _TmpColorBlock;
		//Debug.Log("OffButton-"+_PointingButton+_PointingButton.colors.normalColor+_Pale);

		_TmpImage = _PointingButton.GetComponent<Image>();
		_TmpImage.color = _Pale;
	}
	
	//決定されたゲームオブジェクトのボタンコンポーネント及びイメージコンポーネントの各種colorを_Pressedにする
	void ClickButton(GameObject _TmpObject)
	{
		//Debug.Log("ClickButton");
		_PointingButton = _TmpObject.GetComponent<Button>();
		_TmpColorBlock = _PointingButton.colors;
		_TmpColorBlock.normalColor = _Pressed;
		_TmpColorBlock.highlightedColor = _Pressed;
		_TmpColorBlock.pressedColor = _Pressed;
		_PointingButton.colors = _TmpColorBlock;

		_TmpImage = _PointingButton.GetComponent<Image>();
		_TmpImage.color = _Pressed;
	}
	
	//決定キーが押された後の処理を行う
	void Action()
	{
		if (_SceneName == _End)
		{
			//ゲーム終了
			Debug.Log("ゲーム終了");
			Application.Quit();
		}
		else if (_SceneName == _OptionMode)
		{
			//オプション
			Debug.Log("オプション");
			_OptionObject.SetActive(true);//オプションオブジェクトをアクティブ化
			gameObject.SetActive(false);//ModeSelectオブジェクトを非アクティブ化
		}
		else if (_SceneName != "")
		{
			//指定されたシーンを読み込む
			SceneManager.LoadScene(_SceneName);
		}
	}
}
