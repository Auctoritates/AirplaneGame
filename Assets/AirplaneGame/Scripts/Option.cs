using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour 
{
	//画面描画や処理のための変数
	[SerializeField] private string _OptionName = "";
	[SerializeField] private string _ModeSelectScene = ""; 
	private string _Volume = "";//ボリューム設定用の合図
	private string _End = "";//オプション終了用の合図
	public bool _IsOptionNow = false;//true=オプションメニュー起動許可有り,false=オプションメニュー起動許可無し
	public int _ModeNow = 0;//0=タイトル画面,1=操縦中,2=その他
	[SerializeField] private GameObject _VolumeButton;//ボリュームボタンオブジェクト
	[SerializeField] private GameObject _TitleButton;//タイトルへ戻るボタンオブジェクト
	[SerializeField] private Image _VolumeImage1;
	[SerializeField] private Image _VolumeImage2;
	[SerializeField] private Image _VolumeImage3;
	[SerializeField] private Image _VolumeImage4;
	[SerializeField] private Text _VolumeNumber;
	private Text _OptionText;//読込方法が特殊なためSerializeFieldは使わない
	private float _BGMVolume = 0f;
	private float _SEVolume = 0f;
	private float _VolumeChange = 0f;
	private float _VolumeLevel = 0;
	
	//カーソル移動に関する変数
	private GameObject _ButtonSelected;
	private Color _Bright = Color.clear;
	private Color _Pale = Color.clear;
	private Color _Pressed = Color.clear;
	private Color _Clear = Color.clear;
	private Color _White = Color.clear;
	private int _TimeAfterSelectOption = 0;
	private int _Interval = 0;
	private int _Selected = 0;
	private int _HowMany = 0;
	private bool _IsMovedNow = false;//true=カーソル移動が行われて間もない
	private bool _IsVolumeChangedNow = false;
	public bool _IsSelectedNow = false;//true=タイトルメニュー画面でオプションが選択されて間もない

	//一時的な値を格納する変数
	private string _Code = "";
	private string _TmpString;
	private GameObject _TmpObject;
	private Button _PointingButton;
	private ColorBlock _TmpColorBlock;
	private Image _TmpImage;
	private int _TmpInt;

	void Awake ()//シーン遷移後もオプションオブジェクトを残せるように設定する
	{
		DontDestroyOnLoad (gameObject);
	}
	void OnEnable () 
	{
		//_HowMany = GameObject.FindGameObjectsWithTag("OptionButton").Length;
		_HowMany = 2;//暫定的な処理
		Debug.Log("選択肢個数="+_HowMany);
		
		if(_IsOptionNow)
		{
			OffButton(_VolumeButton);
			OffButton(_TitleButton);

			SetDefalut();

			MenuStart();//オプションメニューの透明度を戻す
			
			SetVolumeEffect();
			
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
			BreakTime(_IsMovedNow);			
		}
		else
		{
			if (Input.GetButton("Cancel"))
			{
				if(_ModeNow == 0)//タイトル画面中の起動なら
				{
					Debug.Log("タイトルへ戻る");
					_Code = _End;
					Action();
				}
				else if(_ModeNow == 1)//ゲーム中なら
				{
					gameObject.SetActive(false);
				}
			}
			else if(Input.GetButton("Submit"))
			{
				ClickButton(_ButtonSelected);
				Action();
			}		
			else if (_TimeAfterSelectOption == 0)//カーソル移動の判定
			{
				//選択キー入力の結果に応じて選択中のボタンIDを変更する
				if (Input.GetAxis("Vertical") > 0.3)
				{
					//Debug.Log("上:Vertical > 0");
					_Selected += -1;
					//Debug.Log("選択中の選択肢ID:"+_Selected);
					_IsMovedNow = true;
					_TimeAfterSelectOption = 1;
				}
				else if (Input.GetAxis("Vertical") < -0.3)
				{
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
		_IsRegular = true;
		
		//1.現在の選択肢個数が1未満
		if(1 > _HowMany)
		{
			_IsRegular = false;
		}
	}
*/
	void MenuStart()
	{
		if(1 > _Selected)
			_Selected = 1;
		else if(_Selected > _HowMany)
			_Selected = _HowMany;
	}
	
	void MenuOff()
	{
		gameObject.SetActive(false);
	}
	
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

	void Action()
	{
		if (_Code == _End)
		{
			Debug.Log("タイトルに戻る");
			gameObject.SetActive(false);

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

	void DecideButton()
	{
		/*
		オプションの項目
			1.音量変更
			2.
			最後.タイトルに戻る
		*/
		
		switch(_Selected)
		{
			case 1:
				//音量設定
				_ButtonSelected = _VolumeButton;
				_Code = _Volume;
				break;
				
			case 2:
				//タイトルに戻るボタン
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

	void OnButton (GameObject _TmpObject)//指定されているボタンを明るくする
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
		_OptionName = "オプション";
		_Volume = "Volume";
		_End = "Title";
		_ModeSelectScene = "ModeSelect";
		
		//変数を初期化
		_Selected = 1;
		_ButtonSelected = _VolumeButton;
		_TimeAfterSelectOption = 0;
		_Interval = 23 + 1;//目的の値に+1する
		_Bright = Color.white;
		_Pale = new Color(0.5f, 1f, 0.5f, 0.5f);
		_Pressed = new Color(0.8f, 0.8f, 0.8f, 1f);
		_White = new Color(1f, 1f, 1f, 1f);
		_BGMVolume = 50f;
		_SEVolume = 50f;
		_VolumeChange = 25f;
		
		//変数にオブジェクトを代入
		_TmpObject = GameObject.Find("OptionText");
		_OptionText = _TmpObject.GetComponent<Text>();
		_OptionText.text = _OptionName;
	}

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

	void VolumeChange()//音量の変更を受け付ける
	{		
		_IsVolumeChangedNow = false;
		//Debug.Log("音量変更受付中:"+_IsVolumeChangedNow);
		if (Input.GetAxis("Horizontal") > 0.3)
		{
			//右
			_IsVolumeChangedNow = true;
			_IsMovedNow = true;
			_TimeAfterSelectOption = 1;
			_BGMVolume += _VolumeChange;
			_SEVolume += _VolumeChange;
			SetVolume();
		}
		else if (Input.GetAxis("Horizontal") < -0.3)
		{
			//左
			_IsVolumeChangedNow = true;
			_IsMovedNow = true;
			_TimeAfterSelectOption = 1;
			_BGMVolume -= _VolumeChange;
			_SEVolume -= _VolumeChange;
			SetVolume();
		}
		//Debug.Log("音量変更後:"+_IsVolumeChangedNow);
		if (_IsVolumeChangedNow)
		{
			SetVolumeEffect();
		}
	}

	void SetVolumeEffect()//全ての音量イメージを消去し音量レベルに合わせてイメージを出現させる
	{
		_VolumeLevel = _BGMVolume / 25f;
		_VolumeNumber.text = _VolumeLevel.ToString();
		Debug.Log("音量レベル"+_VolumeLevel);
		for (_TmpInt = 1; _TmpInt <= 4; _TmpInt++)
		{
			ClearImage(_TmpInt);
		}
		for (_TmpInt = 1; _TmpInt <= _VolumeLevel; _TmpInt++)
		{
			AppearImage(_TmpInt);
		}		
	}

	void ClearImage (int _TmpInt)
	{
		//Debug.Log("ClearImage:Volume"+_TmpInt);
		ImageSelect(_TmpInt);
		_TmpImage.color = _Clear;
	}

	void AppearImage (int _TmpInt)
	{
		//Debug.Log("AppearImage:Volume"+_TmpInt);		
		ImageSelect(_TmpInt);
		_TmpImage.color = _White;
	}

	void ImageSelect(int _TmpInt) //引数の値を参考に処理対象のイメージを_TmpImageへ代入
	{
		switch (_TmpInt)
		{
			case 1:
				_TmpImage = _VolumeImage1;
				break;				
			case 2:
				_TmpImage = _VolumeImage2;
				break;
			case 3:
				_TmpImage = _VolumeImage3;
				break;
			case 4:
				_TmpImage = _VolumeImage4;
				break;
			default:
				break;
		}				
	}
	
	void SetVolume()
	{
		//小数点や上限下限の観点で値を制限する
		Math.Floor(_BGMVolume);
		Math.Floor(_SEVolume);
		if(0f > _BGMVolume || 0f > _SEVolume)
		{	
			_BGMVolume = 0f;
			_SEVolume = 0f;
			_IsVolumeChangedNow = false;
		}
		else if(_BGMVolume > 100f || _SEVolume > 100f)
		{
			_BGMVolume = 100f;
			_SEVolume = 100f;
			_IsVolumeChangedNow = false;
		}
		if (_IsVolumeChangedNow)
		{
			Debug.Log("音量:"+_BGMVolume);
			AudioManager.Instance.ChangeVolume(_BGMVolume/100f, _SEVolume/100f);
		}
	}
}

