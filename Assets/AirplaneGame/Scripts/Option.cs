using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour 
{
	//画面描画や処理のための変数
	[SerializeField] private string _OptionName = "";//オプションの表示名
	[SerializeField] private string _ModeSelectScene = ""; //「タイトルに戻る」で用いるモードセレクト用のシーン名
	private string _Volume = "";//ボリューム設定用の合図
	private string _End = "";//オプション終了用の合図
	public bool _IsOptionNow = false;//true=オプションメニュー起動許可有り,false=オプションメニュー起動許可無し
	public int _ModeNow = 0;//0=タイトル画面,1=操縦中,2=その他
//	private bool _IsRegular;//trueなら正常、falseならUpdateを作動させない
//	[SerializeField] private GameObject _ModeSelectObject;//ゲーム起動直後のシーンのModeSelectオブジェクト
	[SerializeField] private GameObject _VolumeButton;//ボリュームボタンオブジェクト
	[SerializeField] private GameObject _TitleButton;//タイトルへ戻るボタンオブジェクト
	private Text _OptionText;//オプション用テキスト(最前)読込方法が特殊なためSerializeFieldは使わない
	private float _BGMVolume = 0f;//BGMの音量
	private float _SEVolume = 0f;//SEの音量
	private float _VolumeChange = 0f;//音量の変動量
	
	//カーソル移動に関する変数
	private GameObject _ButtonSelected;//現在選択されているボタンのオブジェクト
	private Color _Bright = Color.clear;//選択されているボタン用の明るい色
	private Color _Pale = Color.clear;//選択されていないボタン用の透明度の低い色
	private Color _Pressed = Color.clear;//決定されたボタン用のやや暗い色
	private int _TimeAfterSelectOption = 0;//前回のカーソル移動後の経過時間
	private int _Interval = 0;//カーソル移動後、再びカーソル移動を受け付けるまでの時間設定
	private int _Selected = 0;//現在選択中の選択肢の番号
	private int _HowMany = 0;//選択肢の総数
	private bool _IsMovedNow = false;//true=カーソル移動が行われて間もない
	public bool _IsSelectedNow = false;//true=タイトルメニュー画面でオプションが選択されて間もない

	//一時的な値を格納する変数
	private string _Code = "";//次に何をするかの指令
	[SerializeField] private GameObject _TmpObject;//一時的に指定するゲームオブジェクト
	private Button _PointingButton;//指定したボタンオブジェクトのボタンコンポーネントを格納する変数
	private ColorBlock _TmpColorBlock;//一時的なボタンオブジェクトのボタンコンポーネントのカラーブロック
	private Image _TmpImage;//一時的なオブジェクトのイメージコンポーネント

	void Awake ()//シーン遷移後もオプションオブジェクトを残せるように設定する
	{
		DontDestroyOnLoad (gameObject);//オブジェクトを消去させないようにする
	}
	void OnEnable () 
	{
		//オプション用の選択肢の個数を数える。Tag名は"OptionButton"
		//_HowMany = GameObject.FindGameObjectsWithTag("OptionButton").Length;
		_HowMany = 2;//暫定的な処理
		Debug.Log("選択肢個数="+_HowMany);
		
		if(_IsOptionNow)
		{
			//現在選択されているボタンに応じてボタンのカラーを変更する
			OnButton(_VolumeButton);
			OffButton(_TitleButton);

			//変数の初期値を設定する
			SetDefalut();

			//オプションメニューの透明度を戻す
			MenuStart();

			//問答無用でウェイトを掛ける
			_IsMovedNow = true;
			BreakTime(_IsMovedNow);
		}
	}
	// Update is called once per frame
	void Update () 
	{
		OffButton(_VolumeButton);
		OffButton(_TitleButton);
		DecideButton();

		//カーソル移動受付/ウェイト処理
		if (_TimeAfterSelectOption > 0)
		{
			//前回カーソル移動が成されていた場合ウェイトを掛ける
			BreakTime(_IsMovedNow);			
		}
		else
		{
			//キャンセルキー入力を受け付ける
			if (Input.GetButton("Cancel"))
			{
				if(_ModeNow == 0)//タイトル画面中の起動なら
				{
					//ModeSelectシーンへ戻る
					Debug.Log("タイトルへ戻る");
					_Code = _End;
					Action();
				}
				else if(_ModeNow == 1)//ゲーム中なら
				{
					gameObject.SetActive(false);//このオプションオブジェクトを非アクティブ化
				}
			}
			else if(Input.GetButton("Submit"))//決定キー入力を受け付ける
			{
				//対象のボタンの色を変更する
				ClickButton(_ButtonSelected);
				//決定かクリックされたボタンに応じて処理を行う
				Action();
			}		
			else if (_TimeAfterSelectOption == 0)//カーソル移動の判定
			{
				/*上下カーソル移動*/
				//選択キー入力の結果に応じて選択中のボタンIDを変更する
				if (Input.GetAxis("Vertical") > 0.3)
				{
					//上
					//Debug.Log("上:Vertical > 0");
					_Selected += -1;
					//Debug.Log("選択中の選択肢ID:"+_Selected);
					_IsMovedNow = true;
					_TimeAfterSelectOption = 1;
				}
				else if (Input.GetAxis("Vertical") < -0.3)
				{
					//下
					//Debug.Log("下:Vertical < 0");
					_Selected += 1;
					//Debug.Log("選択中の選択肢ID:"+_Selected);
					_IsMovedNow = true;
					_TimeAfterSelectOption = 1;
				}

				//選択中のボタンIDの上限と下限を設け、超えていた場合逆の閾値にする
				if (_Selected < 1)
				{
					_Selected = _HowMany;
					//Debug.Log("選択中の選択肢ID:"+_Selected);
				}
				else if (_Selected > _HowMany)
				{
					_Selected = 1;
					//Debug.Log("選択中の選択肢ID:"+_Selected);
				}

				if(_Code == _Volume)
				{
					VolumeChange();
				}
			}
		}
	}

/*
	void DitectIrregular()
	{
		//不都合な状況であればfalseを返してUpdateを停止させる
		_IsRegular = true;
		
		//1.現在の選択肢個数が1未満
		if(1 > _HowMany)
		{
			_IsRegular = false;
			//Debug.Log("選択肢数が1未満の為中断");
		}
	}
*/
	void MenuStart()//現在の選択中の番号の上限と下限を設定する。
	{
		if(1 > _Selected)//選択中ボタン番号の上限下限設定
			_Selected = 1;
		else if(_Selected > _HowMany)
			_Selected = _HowMany;
	}
	
	void MenuOff()
	{
		//オプションメニューUIオブジェクトを非アクティブにする
		gameObject.SetActive(false);//このオプションオブジェクトを非アクティブ化
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

	void BreakTime(bool _IsPushed)//_Interval分のウェイトが経過していたらウェイトを解除し、それ以外ではウェイトを発生させる
	{
		if (_IsPushed)
		{
			//Debug.Log("カーソル操作ウェイト");	
			if (_TimeAfterSelectOption >= _Interval)
			{
				_IsPushed = false;
				_TimeAfterSelectOption = 0;
				return;
			}
			else
			{
				_TimeAfterSelectOption += 1;
			}
		}
	}

	//決定キーが押された後の処理を行う
	void Action()
	{
		if (_Code == _End)
		{
			//タイトルに戻る
			Debug.Log("タイトルに戻る");
			gameObject.SetActive(false);//このオプションオブジェクトを非アクティブ化

			if(_ModeNow == 0)
			{
				_IsOptionNow = false;
			}
			else if (_ModeNow == 1)
			{
				SceneManager.LoadScene(_ModeSelectScene);
			}
		}
	}

	void DecideButton()//現在選択されているボタンのみを光らせる
	{
		/*オプションの項目
		1.音量変更
		2.
		最後.タイトルに戻る
		*/
		
		//選択IDから現在選択中のボタンを決定する
		switch(_Selected)
		{
			case 1://音量設定
				_ButtonSelected = _VolumeButton;
				_Code = _Volume;
				break;
				
			case 2://タイトルに戻るボタン
				_ButtonSelected = _TitleButton;
				_Code = _End;
				break;
				
			default:
				_Selected = 1;
				_ButtonSelected = _VolumeButton;
				break;
		}
		OnButton(_ButtonSelected);
	}
	
	void VolumeChange()//音量の変更を受け付ける
	{		
		/*左右カーソル移動判定*/
		if (Input.GetAxis("Horizontal") > 0.3)
		{
			//右
			_IsMovedNow = true;
			_TimeAfterSelectOption = 1;
			_BGMVolume += _VolumeChange;
			_SEVolume += _VolumeChange;
			SetVolume();
		}
		else if (Input.GetAxis("Horizontal") < -0.3)
		{
			//左
			_IsMovedNow = true;
			_TimeAfterSelectOption = 1;
			_BGMVolume -= _VolumeChange;
			_SEVolume -= _VolumeChange;
			SetVolume();
		}
	}

	//指定されているボタンを明るくする
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
	void SetDefalut()//各変数の初期値を設定する
	{
		//文字列を初期化
		_OptionName = "オプション";//このゲームのタイトル名を設定
		_Volume = "Volume";//ボリューム設定用の合図となる言葉
		_End = "Title";//タイトルに戻る用の合図となる言葉
	
		//変数を初期化
		_Selected = 1;//選択肢番号は1～_HowManyまでの値であれば初期化しない
		_ButtonSelected = _VolumeButton;
		_TimeAfterSelectOption = 0;
		_Interval = 23 + 1;//目的の値に+1する
		_Bright = Color.white;//全ての値が1f(FF)、つまりFFFFFFFF、はっきりした白色
		_Pale = new Color(0.5f, 1f, 0.5f, 0.5f);//透明度のみ0.5f、薄い白色
		_Pressed = new Color(0.8f, 0.8f, 0.8f, 1f);//やや暗い白
		_BGMVolume = 50f;
		_SEVolume = 50f;
		_VolumeChange = 25f;//音量の変動量
		
		//ModeSelectシーン名を設定する
		_ModeSelectScene = "ModeSelect";

		//変数にオブジェクトを代入
		_TmpObject = GameObject.Find("OptionText");
		_OptionText = _TmpObject.GetComponent<Text>();//タイトルテキストオブジェクトのテキストコンポーネントを取得
		_OptionText.text = _OptionName;//.textは小文字限定という初見お断り仕様
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

	void SetVolume()//変更した音量を格納する
	{
		Math.Floor(_BGMVolume);
		Math.Floor(_SEVolume);
		if(0f > _BGMVolume || 0f > _SEVolume)
		{	
			_BGMVolume = 0f;
			_SEVolume = 0f;
		}
		else if(_BGMVolume > 100f || _SEVolume > 100f)
		{
			_BGMVolume = 100f;
			_SEVolume = 100f;
		}
		Debug.Log(":音量:"+_BGMVolume);
		AudioManager.Instance.ChangeVolume(_BGMVolume/100f, _SEVolume/100f);
	}
}

