using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityFramework;

public class PanelExam : MonoBehaviour
{
	public UIForm_ChoiceQuestion UIForm_ChoiceQuestion;
	public Text mQuestText;
	public Text NowQuestionNum;


	public List<Part_OptionToggle> mOptionToggles;

	public Button mSubmitBtn;
	public Button mNextBtn;
	public Button mConfirm;
	public GameObject mExamRoot;
	string rightAmswer;
	public Text TextSelect;
	public Text TextRight;

	internal List<string> SelectList = new List<string>();

	internal List<string> errorQuestion = new List<string>();



	/// <summary>
	//所有题目
	/// </summary>
	List<QuestionData> examConfs = new List<QuestionData>();
	/// <summary>
	// 要做的题目
	/// </summary>
	List<QuestionData> toDoExams = new List<QuestionData>();
	/// <summary>
	// 每一题的选项
	/// </summary>
	Dictionary<QuestionData, List<string>> selectedOptions = new Dictionary<QuestionData, List<string>>();

	int AllNum = 0;
	int errornum = 0;

	int index = 0;

	// Start is called before the first frame update
	void Start()
	{
		Messenger.AddListener<string, string, bool>("SelectOption", OnOptionSelected);

		mNextBtn.onClick.AddListener(delegate
	   {
		   this.OnNextBtnClicked();
	   });

		mSubmitBtn.onClick.AddListener(delegate
	   {
		   this.OnSubmitBtnClicked();
	   });


		mConfirm.onClick.AddListener(delegate
		{
			this.OnConfirmBtnClicked();
		});
	}



	public void InitUI(List<QuestionData> _examConfs)
	{
		InitData(_examConfs);

		TextSelect.text = "";
		TextRight.text = "";
		mNextBtn.gameObject.SetActive(false);
		mSubmitBtn.gameObject.SetActive(true);
		mConfirm.gameObject.SetActive(false);

		ShowUI();

	}

	void InitData(List<QuestionData> _examConfs)
	{
		index = 0;
		examConfs.Clear();
		toDoExams.Clear();
		selectedOptions.Clear();
		AllNum = 0;
		errornum = 0;
		examConfs = _examConfs;
		foreach (QuestionData item in examConfs)
		{
			toDoExams.Add(item);
		}



	}

	void ShowUI()
	{
		mExamRoot.SetActive(true);
		FreshExam();

	}


	void FreshExam()
	{
		if (index < toDoExams.Count)
		{
			TextRight.text = "";
			TextSelect.text = "";
			SelectList.Clear();
			rightAmswer = null;
			mQuestText.text = toDoExams[index].questionContent;
			AllNum = toDoExams.Count;
			NowQuestionNum.text = $"第{index + 1}题共{AllNum}";
			for (int i = 0; i < toDoExams[index].optionList.Length; i++)
			{
				mOptionToggles[i].InitUI(toDoExams[index].optionList[i]);
			}
			List<string> list = toDoExams[index].answerList.ToList();
			list.Sort();
			foreach (var item in list)
			{
				rightAmswer += item;
			}
		}
	}


	void NextExam()
	{
		if (index < toDoExams.Count - 1)
		{
			index++;
			FreshExam();
			mNextBtn.gameObject.SetActive(false);
			mSubmitBtn.gameObject.SetActive(true);
			SetOptionToggleInteractable(true);
		}


	}

	void Submit()
	{
		foreach (KeyValuePair<QuestionData, List<string>> kvp in selectedOptions)
		{
			//answers 正确答案
			List<string> answers = kvp.Key.answerList.ToList<string>();
			if (answers.Count == kvp.Value.Count)
			{
				foreach (var item in kvp.Value)
				{
					if (!answers.Contains(item))
					{
						errornum++;
						return;
					}
				}

			}
			else
			{
				errornum++;
			}
		}


	}

	void OnOptionSelected(string option, string _selectedOption, bool isOn)
	{
		if (!gameObject.activeInHierarchy)
		{
			return;
		}
		if (isOn)
		{
			if (!SelectList.Contains(option))
			{
				SelectList.Add(option);
			}

		}
		else
		{
			if (SelectList.Contains(option))
			{
				SelectList.Remove(option);
			}
		}
		TextSelect.text = "";
		SelectList.Sort();
		foreach (var item in SelectList)
		{
			TextSelect.text += item;
		}
		CacheAnswer(option, isOn);

	}

	/// <summary>
	/// 缓存答案
	/// </summary>
	/// <param name="_selectedOption"></param>
	void CacheAnswer(string _selectedOption, bool isOn)
	{
		if (selectedOptions.ContainsKey(toDoExams[index]))
		{
			//toggle选中
			if (isOn)
			{
				if (!selectedOptions[toDoExams[index]].Contains(_selectedOption))
				{
					// 取消选项
					selectedOptions[toDoExams[index]].Add(_selectedOption);
				}
			}
			else
			{
				if (selectedOptions[toDoExams[index]].Contains(_selectedOption))
				{
					// 取消选项
					selectedOptions[toDoExams[index]].Remove(_selectedOption);
				}
			}

		}
		else
		{
			selectedOptions.Add(toDoExams[index], new List<string>() { _selectedOption });
		}
	}

	bool CheckAnswer(string _selectedOption)
	{
		for (int i = 0; i < toDoExams[index].answerList.Length; i++)
		{
			if (toDoExams[index].answerList[i].Equals(_selectedOption))
			{
				return true;
			}
		}
		return false;
	}

	void OnSubmitBtnClicked()
	{
		TextRight.text = rightAmswer;
		if (!selectedOptions.ContainsKey(toDoExams[index]))
		{
			selectedOptions.Add(toDoExams[index], new List<string>() { });
		}
		SetOptionToggleInteractable(false);
		mSubmitBtn.gameObject.SetActive(false);
		if (index < toDoExams.Count - 1)
		{
			mNextBtn.gameObject.SetActive(true);
		}
		else
		{
			mConfirm.gameObject.SetActive(true);
		}
	}

	void OnNextBtnClicked()
	{
		NextExam();



	}
	private void OnConfirmBtnClicked()
	{
		Submit();
		string content = $"共{AllNum}题,错误{errornum}题";
		UIForm_ChoiceQuestion.AllNum = AllNum;
		UIForm_ChoiceQuestion.errornum = errornum;

		UIForm_ChoiceQuestion.SetInfoText(content);

	}

	void SetOptionToggleInteractable(bool boolean)
	{
		foreach (var item in mOptionToggles)
		{
			item.GetComponent<Button>().interactable = boolean;
		}
	}


	public void ShowPanel()
	{
		gameObject.SetActive(true);
	}

	public void HidePanel()
	{
		gameObject.SetActive(false);
	}


	private void OnDestroy()
	{
		if (Messenger.eventTable.ContainsKey("SelectOption"))
		{
			Messenger.RemoveListener<string, string, bool>("SelectOption", OnOptionSelected);
		}
	}





}
