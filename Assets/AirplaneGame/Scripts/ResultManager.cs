using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour 
{
	[SerializeField] private GameObject _ResultUI;
	[SerializeField] private Image _ResultBackImage1;
	[SerializeField] private Image _ResultBackImage2;
	[SerializeField] private Text _ClearStringText;
	[SerializeField] private Text _GetSStringText;
	[SerializeField] private Text _GetSNumberText;
	[SerializeField] private Text _TotalSStringText;
	[SerializeField] private Text _TotalSNumberText;
 	public string _ActionCode;
	public bool _IsResultOK;

	private Image _TmpImage;
	private Text _TmpText;
	private Color _TmpColor;
	private float _Alpha;
	// Use this for initialization
	void Start () 
	{
		SetDefault();
		SetResultOff();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_IsResultOK)
		{
			SetResultOn();
		}
		Action();
		if (_ActionCode == "End")
		{
			SetResultOff();
		}
	}

	void SetDefault()
	{
		_ActionCode = "";
		_IsResultOK = false;
	}
	void SetResultOn()
	{
		_ResultUI.SetActive(true);
		_Alpha = 255f;
//		SetColor();
		}
	void SetResultOff()
	{
		_Alpha = 0f;
//		SetColor();		
		_ResultUI.SetActive(false);				
	}
	void SetColor()
	{
		ImageColor(_ResultBackImage1);
		ImageColor(_ResultBackImage2);
		TextColor(_GetSNumberText);
		TextColor(_GetSStringText);		
		TextColor(_TotalSStringText);
		TextColor(_TotalSNumberText);
	}
	void ImageColor(Image _TmpImage)
	{
		_TmpColor = _TmpImage.color;
		_TmpColor.a = _Alpha;
		_TmpImage.color = _TmpColor;		
	}
	void TextColor(Text _TmpText)
	{
		_TmpColor = _TmpText.GetComponent<Text>().color;
		_TmpColor.a = _Alpha;
		_TmpText.GetComponent<Text>().color = _TmpColor;
	}
	void Action()
	{
		if (Input.GetButton("Submit"))
		{
			_ActionCode = "End";
		}
	}
}