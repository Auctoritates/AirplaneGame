using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeSelect : MonoBehaviour 
{
	//変数を宣言
	//モードセレクト画面のオブジェクト用の変数
	Option _OptionScript;//オプションメニューUI総合オブジェクトのスクリプト
	[SerializeField] private string _TitleName = "";//このゲームのタイトル名
	[SerializeField] private string _SceneGame = "";//ゲームシーンのファイル名
	private string _OptionMode = "";//オプション用の合図
	private string _End = "";//ゲーム終了用の合図
	[SerializeField] private GameObject _OptionObject;//オプションオブジェクト
	[SerializeField] private GameObject _StartButton;//スタートボタンオブジェクト
	[SerializeField] private GameObject _OptionButton;//オプションボタンオブジェクト
	[SerializeField] private GameObject _EndButton;//終了ボタンオブジェクト
	[SerializeField] private Image _ModeSelectImage1; //クリア時の演出イメージ背景黒
	[SerializeField] private Text _TitleText;//

	//カーソル移動に関する変数
	private GameObject _ButtonSelected;//現在選択されているボタンのオブジェクト
	private Color _Bright = Color.clear;//選択されているボタン用の明るい色
	private Color _Pale = Color.clear;//選択されていないボタン用の透明度の低い色
	private Color _Pressed = Color.clear;//決定されたボタン用のやや暗い色
	private Color _Off = Color.clear;//非表示用の黒色透明
	private Color _BlueBack = Color.clear;//
	private Color _TextColor = Color.clear;//
	private int _SelectedNumber = 0;//現在選択されているボタンのID 0=スタートボタン, 1=オプションボタン, 2=終了ボタン
	private int _TimeAfterSelectStartMenu = 0;//前回のカーソル移動後の経過時間
	private int _Interval = 0;//カーソル移動後、再びカーソル移動を受け付けるまでの時間設定
	private bool _IsMovedNow = false;//true=カーソル移動が行われて間もない
	private bool _IsMenuOK = false;//true = メニューの描画が完了した
	private bool _IsOptionNow = false;//true=オプションメニュー,false=モードセレクトメニュー
	//一時的な値を格納する変数
	private string _Code = "";//次に行う行動の指示コード格納変数
	private GameObject _TmpObject;//一時的に指定するゲームオブジェクト
	private Button _PointingButton;//指定したボタンオブジェクトのボタンコンポーネントを格納する変数
	private ColorBlock _TmpColorBlock;//一時的なボタンオブジェクトのボタンコンポーネントのカラーブロック
	private Image _TmpImage;//一時的なオブジェクトのイメージコンポーネント

	// Use this for initialization
	void Start () 
	{
		_IsOptionNow = false;
		_IsMenuOK = false;
		//オブジェクトの色を設定
		_BlueBack = new Color(2f/255f, 201f/255f, 253f/255f, 255f/255f);//Colorの値は0～1の値に設定する。
		_TextColor = new Color(50f/255f,50f/255f,50f/255f,255f/255f);
		//Optionオブジェクトの情報取得
		_OptionObject = GameObject.Find("Option");
		_OptionScript = _OptionObject.GetComponent<Option>();
		_OptionObject.SetActive(false);


		//初期化
		_TitleName = "フライト";//このゲームのタイトル名を設定
		_SceneGame = "GamePlay";//ゲーム用のシーンのファイル名(拡張子抜き)
		_OptionMode = "Option";//オプションオブジェクトの名前 またオプションの合図となる言葉
		_End = "End";//ゲーム終了用の合図となる言葉
		//UIオブジェクトを取得
		//キャンバス内のUIパーツの詳細はコンポーネント扱い
		//タイトルテキストの内容を書き換える
		_TmpObject = GameObject.Find("TitleText");
		_TitleText = _TmpObject.GetComponent<Text>();//タイトルテキストオブジェクトのテキストコンポーネントを取得
		_TitleText.text = _TitleName;//.textは小文字限定という初見お断り仕様
		
		//カーソル移動に関する変数を初期化
		_SelectedNumber = 0;//現在選択中のボタンをスタートボタンに設定
		_ButtonSelected = _StartButton;
		_Interval = 30 + 1;//目的の値に+1する
		_TimeAfterSelectStartMenu = 0;
		_Bright = Color.white;//全ての値が1f(FF)、つまりFFFFFFFF、はっきりした白色
		_Pale = new Color(0.5f, 1f, 0.5f, 0.5f);//透明度のみ0.5f、薄い白色
		_Pressed = new Color(0.8f, 0.8f, 0.8f, 1f);//やや暗い白
	}
	
	// Update is called once per frame
	void Update () 
	{
		_IsOptionNow = _OptionScript._IsOptionNow;
		if (_IsOptionNow)
		{
			//オプションメニュー中なら
			//自己を非表示にする
			StartMenuEnd();
		}
		else
		{
			StartMenuBegin();

			if(_IsMenuOK)
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
						_Code = _SceneGame;
						break;
				
					case 1:
						_ButtonSelected = _OptionButton;
						_Code = _OptionMode;
						break;
				
					case 2:
						_ButtonSelected = _EndButton;
						_Code = _End;//ゲーム終了の合図
						break;
				
					default:
						_SelectedNumber = 0;
						_ButtonSelected = _StartButton;
						_Code = _SceneGame;
						break;
				}
				OnButton(_ButtonSelected);

				if (_TimeAfterSelectStartMenu > 0)
				{
					//カーソル操作が行われていたらウェイトを掛ける
					//Debug.Log("カーソル移動ウェイトを呼び出す");
					BreakTime(_IsMovedNow);
				}		
				else
				{
					//決定キー入力を受け付ける
					if(Input.GetButton("Submit"))
					{
						//Debug.Log("決定キー");
						//対象のボタンの色を変更する
						ClickButton(_ButtonSelected);
						//決定かクリックされたボタンに応じて処理を行う
						Action();
					}
					//カーソル移動の判定
					if (_TimeAfterSelectStartMenu == 0)
					{
						//選択キー入力の結果に応じて選択中のボタンIDを変更する
						if (Input.GetAxis("Vertical") > 0)
						{
							//上
							//Debug.Log("上:Vertical < 0");
							_SelectedNumber += -1;
							_IsMovedNow = true;
							_TimeAfterSelectStartMenu = 1;
						}
						else if (Input.GetAxis("Vertical") < 0)
						{
							//下
							_SelectedNumber += 1;
							_IsMovedNow = true;
							_TimeAfterSelectStartMenu = 1;
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
			}
		}
	}
	
	//指定されたゲームオブジェクトのボタンコンポーネント及びイメージコンポーネントを明るいカラーにする
	/*
		カラーはカラーブロックの代入で編集できる
		カラーはImageコンポーネントのcolorとButtonコンポーネントのnormalColorの2項目の乗算で描かれている
	*/
	void StartMenuBegin ()
	{
		//
		if(_IsOptionNow == false)
		{	
			if(_IsMenuOK == false)
			{
				//Debug.Log("初期メニュー再描画");
				//オブジェクトを表示状態にする
				_StartButton.SetActive(true);
				_OptionButton.SetActive(true);
				_EndButton.SetActive(true);
				_ModeSelectImage1.GetComponent<Image>().color = _BlueBack;
				_TitleText.GetComponent<Text>().color = _TextColor;
	
				BreakTime(true);//問答無用でウェイトを掛ける
				_IsMovedNow = true;
			}
		}
	}
	void StartMenuEnd ()
	{
		//ボタンの非アクティブ化
		ClearButton(_StartButton);
		ClearButton(_OptionButton);
		ClearButton(_EndButton);
		//イメージの非アクティブ化
		_ModeSelectImage1.GetComponent<Image>().color = _Off;
		//タイトルテキストの非アクティブ化
		_TitleText.GetComponent<Text>().color = _Off;
	}

	void ClearButton (GameObject _TmpObject)
	{
		//Debug.Log("ClearButton");
		_TmpObject.SetActive(false);
	}
	
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

	void BreakTime(bool _IsPushed)
	{
		//カーソル移動や選択が成された後に呼び出され、ウェイトをかける
		if (_IsPushed)
		{
			//Debug.Log("カーソル操作ウェイト");	
			if (_TimeAfterSelectStartMenu >= _Interval)
			{
				_IsPushed = false;
				_IsMenuOK = true;
				_TimeAfterSelectStartMenu = 0;
				return;
			}
			else
			{
				_TimeAfterSelectStartMenu += 1;
				//Debug.Log("現在ウェイト数:"+_TimeAfterSelectStartMenu);
			}
		}
	}
	
	//決定キーが押された後の処理を行う
	void Action()
	{
		if (_Code == _End)
		{
			//ゲーム終了
			//Debug.Log("ゲーム終了");
			Application.Quit();
		}
		else if (_Code == _OptionMode)
		{
			//オプション
			Debug.Log("オプション");
			_IsMenuOK = false;
			StartMenuEnd();
			_IsOptionNow = true;
			_OptionScript._IsOptionNow = _IsOptionNow;//
			_OptionObject.SetActive(true);//オプションオブジェクトをアクティブ化
		}
		else if (_Code != "")
		{
			//指定されたシーンを読み込む
			SceneManager.LoadScene(_Code);
		}
	}
}
