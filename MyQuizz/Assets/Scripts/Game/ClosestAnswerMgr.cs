using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[XmlRoot("DataClosestAnswer")]
public class DataQuestion
{
	[XmlAttribute("questionList")]
	public List<string> questionList = new List<string>();
	[XmlAttribute("answerList")]
	public List<string> answerList = new List<string>();
}

public class ClosestAnswerMgr : MonoBehaviour
{
	private void Start()
	{
		nextQuestionButton.onClick.AddListener(NextQuestion);
		previousQuestionButton.onClick.AddListener(PreviousQuestion);
		showAnswerButton.onClick.AddListener(ShowAnswer);

		InitGame();
		InitEditor();
	}

	#region GameVariables

	[SerializeField]
	TextMeshProUGUI questionText;
	[SerializeField]
	TextMeshProUGUI answerText;
	[SerializeField]
	Button nextQuestionButton;
	[SerializeField]
	Button previousQuestionButton;

	[SerializeField]
	GameObject answerGameObject;
	[SerializeField]
	Button showAnswerButton;

	DataQuestion data = new DataQuestion();

	int currentQuestion = 0;

	#endregion

	#region GameMethods

	public void InitGame()
	{
		Load();

		currentQuestion = 0;
		ResetQuestionButton();
		HideAnswer();

		if (data.questionList.Count > 0)
			UpdateQuestion();
	}

	private void ResetQuestionButton()
	{
		previousQuestionButton.interactable = false;
		nextQuestionButton.interactable = (data.questionList.Count > 1) ? true : false;
	}

	public void UpdateQuestion()
	{
		questionText.text = data.questionList[currentQuestion];
		answerText.text = data.answerList[currentQuestion];
	}

	public void NextQuestion()
	{
		currentQuestion++;
		HideAnswer();
		UpdateQuestion();

		if (currentQuestion >= data.questionList.Count - 1)
			nextQuestionButton.interactable = false;
		
		previousQuestionButton.interactable = true;
	}

	public void PreviousQuestion()
	{
		currentQuestion--;
		HideAnswer();
		UpdateQuestion();

		if (currentQuestion <= 0)
			previousQuestionButton.interactable = false;

		nextQuestionButton.interactable = true;
	}

	public void ShowAnswer()
	{
		answerGameObject.SetActive(true);
		showAnswerButton.gameObject.SetActive(false);
	}

	public void HideAnswer()
	{
		answerGameObject.SetActive(false);
		showAnswerButton.gameObject.SetActive(true);
	}

	#endregion

	#region EditorVariables

	[SerializeField]
	TMP_InputField questionField;
	[SerializeField]
	TMP_InputField answerField;

	#endregion

	#region EditorMethods 

	public void InitEditor()
	{
		currentQuestion = 0;
		questionField.text = data.questionList[currentQuestion];
		answerField.text = data.answerList[currentQuestion];
	}

	public void AddQuestion()
	{
		data.questionList.Add(questionField.text);
		data.answerList.Add(answerField.text);
		NextEdit();
	}

	public void RemoveQuestion()
	{
		data.questionList.RemoveAt(currentQuestion);
		data.answerList.RemoveAt(currentQuestion);
		UpdateInputEdit();
	}

	public void NextEdit()
	{
		if (currentQuestion < data.questionList.Count)
		{
			currentQuestion++;
			UpdateInputEdit();
		}
	}
	public void PreviousEdit()
	{
		if (currentQuestion > 0)
		{
			currentQuestion--;
			UpdateInputEdit();
		}
	}

	public void UpdateInputEdit()
	{
		if (currentQuestion < data.questionList.Count && currentQuestion >= 0)
		{
			questionField.text = data.questionList[currentQuestion];
			answerField.text = data.answerList[currentQuestion];
			return;
		}

		questionField.text = "";
		answerField.text = "";
	}

	private void RemoveSpace()
	{
		for (int i = 0; i < data.questionList.Count; i++)
		{
			data.questionList[i] = SaveMgr.RemoveSpace(data.questionList[i]);
			data.answerList[i] = SaveMgr.RemoveSpace(data.answerList[i]);
		}
	}

	private void RecoverSpace()
	{
		for (int i = 0; i < data.questionList.Count; i++)
		{
			data.questionList[i] = SaveMgr.RecoverSpace(data.questionList[i]);
			data.answerList[i] = SaveMgr.RecoverSpace(data.answerList[i]);
		}
	}

	#endregion

	#region SaveAndLoad

	public void Save()
	{
		RemoveSpace();
		SaveMgr.Serialize(data, StaticVariables.filePathClosestAnswer);
	}

	public void Load()
	{
		DataQuestion _data = SaveMgr.Deserialize<DataQuestion>(StaticVariables.filePathClosestAnswer);
		if (_data != null)
		{
			data = _data;
			RecoverSpace();
		}
	}

	#endregion
}
