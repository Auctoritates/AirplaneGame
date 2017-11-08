using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour 
{
	//画面描画や処理のための変数
	public string _OptionName = "";//オプションの表示名
	public string _ModeSelectScene = ""; //「タイトルに戻る」で用いるモードセレクト用のシーン名
	private string _Volume = "";//ボリューム設定用の合図
	private string _End = "";//オプション終了用の合図
	private bool _IsRegular;//trueなら正常、falseならUpdateを作動させない
	//private GameObject _OptionUI;//オプションUI総合
	public GameObject _VolumeButton;//ボリュームボタンオブジェクト
	public GameObject _TitleButton;//タイトルへ戻るボタンオブジェクト
	public Text _OptionText;//オプション表記テキスト(最前)
	
	//カーソル移動に関する変数
	private GameObject _ButtonSelected;//現在選択されているボタンのオブジェクト
	private Color _Bright = Color.clear;//選択されているボタン用の明るい色
	private Color _Pale = Color.clear;//選択されていないボタン用の透明度の低い色
	private Color _Pressed = Color.clear;//決定されたボタン用のやや暗い色
	private int _TimeAfterSelect = 0;//前回のカーソル移動後の経過時間
	private int _Interval = 0;//カーソル移動後、再びカーソル移動を受け付けるまでの時間設定
	private int _Selected = 0;//現在選択中の選択肢の番号
	private int _HowMany = 0;//選択肢の総数
	private bool _IsSelectedNow = false;//true=カーソル移動が行われて間もない

	//一時的な値を格納する変数
	private string _Code = "";//次に何をするかの指令
	public GameObject _TmpObject;//一時的に指定するゲームオブジェクト
	private Button _PointingButton;//指定したボタンオブジェクトのボタンコンポーネントを格納する変数
	private ColorBlock _TmpColorBlock;//一時的なボタンオブジェクトのボタンコンポーネントのカラーブロック
	private Image _TmpImage;//一時的なオブジェクトのイメージコンポーネント

	// Use this for initialization
	void Start () 
	{
		//オプション用の選択肢の個数を数える。Tag名は"OptionButton"
		//_HowMany = GameObject.FindGameObjectsWithTag("OptionButton").Length;
		_HowMany = 2;//暫定的な処理
		Debug.Log("選択肢個数="+_HowMany);

		//現在選択されているボタンに応じてボタンのカラーを変更する
		OffButton(_VolumeButton);
		OffButton(_TitleButton);
		
		//文字列を初期化
		_OptionName = "オプション";//このゲームのタイトル名を設定
		_Volume = "Volume";//ボリューム設定用の合図となる言葉
		_End = "Title";//タイトルに戻る用の合図となる言葉
		
		//変数を初期化
		_Selected = 1;//選択肢番号は1～_HowManyまでの値であれば初期化しない
		_ButtonSelected = _VolumeButton;
		_TimeAfterSelect = 0;
		_Interval = 30 + 1;//目的の値に+1する
		_IsSelectedNow = false;
		_Bright = Color.white;//全ての値が1f(FF)、つまりFFFFFFFF、はっきりした白色
		_Pale = new Color(0.5f, 1f, 0.5f, 0.5f);//透明度のみ0.5f、薄い白色
		_Pressed = new Color(0.8f, 0.8f, 0.8f, 1f);//やや暗い白
		
		//ModeSelectシーン名を設定する
		_ModeSelectScene = "ModeSelect";

		//変数にオブジェクトを代入
		//_OptionUI = GameObject.Find("OptionUI");//オプションUIを総合したオブジェクトを登録する
/*		_VolumeButton = GameObject.Find("VolumeButton");//スタートボタンのオブジェクトを取得
		_TitleButton = GameObject.Find("TitleButton");

		_TmpObject = GameObject.Find("OptionText");
		_OptionText = _TmpObject.GetComponent<Text>();//タイトルテキストオブジェクトのテキストコンポーネントを取得
		_OptionText.text = _OptionName;//.textは小文字限定という初見お断り仕様
*/
		//オプションメニューの透明度を戻す
		MenuStart();
	}
	
	// Update is called once per frame
	void Update () 
	{
		OffButton(_VolumeButton);
		OffButton(_TitleButton);

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
		//キャンセルキー入力を受け付ける
		if (Input.GetButton("Cancel"))
		{
			if(true)//タイトル画面からの派生なら
			{
				//ModeSelectシーンへ戻る
				Debug.Log("タイトルへ戻る");
				SceneManager.LoadScene(_ModeSelectScene);
			}
			else//ゲーム中なら
			{
				MenuOff();
			}
		}
		//決定キー入力を受け付ける
		if(Input.GetButton("Submit"))
		{
			//対象のボタンの色を変更する
			ClickButton(_ButtonSelected);
			//決定かクリックされたボタンに応じて処理を行う
			Action();
		}		
/*		
		//ゲームステージ中かつキャンセルキーが押されたらメニュー描画前に戻す
		//タイトル画面でのキャンセルキーならタイトルに戻る

		//決定キーならswitch文で選択肢ごとに別の挙動をする
		if (Input.GetButton("Submit"))
		{
			switch (_Selected)
			{
				case 1:
					//1.音量変更
					break;
		
				case 2:
					//2.
					break;
			
				case _HowMany:
					//最後.タイトルに戻る
					//ModeSelectシーンへ戻る
					Debug.Log("タイトルへ戻る");
					SceneManager.LoadScene(_ModeSelectScene);
					break;

				default:
					//その他.メニューを終了する
					MenuOff();
					break;
			}
		}
	*/		
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
				_Selected += -1;
				_IsSelectedNow = true;
				_TimeAfterSelect = 1;
			}
			else if (Input.GetAxis("Vertical") < 0)
			{
				//下
				_Selected += 1;
				_IsSelectedNow = true;
				_TimeAfterSelect = 1;
			}

			//選択中のボタンIDの上限と下限を設け、超えていた場合逆の閾値にする
			if (_Selected < 0)
			{
				_Selected = _HowMany;
			}
			else if (_Selected > _HowMany)
			{
				_Selected = 1;
			}
		}
	}

	void DitectIrregular()
	{
		//不都合な状況であればfalseを返してUpdateを停止させる
		_IsRegular = true;
		
		//1.現在の選択肢個数が1未満
		if(1 > _HowMany)
		{
			_IsRegular = false;
			Debug.Log("選択肢数が1未満の為中断");
		}
	}
	void MenuStart()
	{
		//メニューUIオブジェクトをアクティブにし、非透明度を255にする。
		//さらに現在の選択中の番号の上限と下限を設定する。

		//_OptionUI.SetActive(true);//アクティブ化
		
		if(1 > _Selected)//選択中ボタン番号の上限下限設定
			_Selected = 1;
		else if(_Selected > _HowMany)
			_Selected = _HowMany;
	}
	void MenuOff()
	{
		//メニューUIオブジェクトを非透明度を0にし、非アクティブにする
		//_OptionUI.SetActive(false);
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
		if (_Code == _End)
		{
			//タイトルに戻る
			Debug.Log("タイトルに戻る");

		}
	}
	
}
