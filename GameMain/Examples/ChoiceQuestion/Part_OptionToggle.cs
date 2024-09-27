using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Part_OptionToggle : MonoBehaviour
{
	public Button mButton;
	public Image mSelect;
	public Text mText;

	private string key;
	public string option;


	private bool isSelect = false;

	// Start is called before the first frame update
	void Start()
	{
		mButton.onClick.AddListener(delegate
	   {
		   this.OnToggleBtnClicked();
	   });
		isSelect = false;
		mSelect.gameObject.SetActive(isSelect);
	}

	public void OnToggleBtnClicked()
	{
		isSelect = !isSelect;
		mSelect.gameObject.SetActive(isSelect);
		Messenger.Broadcast<string, string, bool>("SelectOption", option, key, isSelect);
	}

	public void InitUI(string _key)
	{
		key = _key;
		if (string.IsNullOrEmpty(_key))
		{
			gameObject.SetActive(false);
		}
		else
		{
			mSelect.gameObject.SetActive(false);
			isSelect = false;
			gameObject.SetActive(true);
			  mText.text =_key;
		}
	}
}
