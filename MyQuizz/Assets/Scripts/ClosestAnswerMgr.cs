using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClosestAnswerMgr : MonoBehaviour
{
	#region Game

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

	[SerializeField]
	DataClosestAnswer data = new DataClosestAnswer();

	int currentQuestion = 0;

	public void InitGame()
	{
		currentQuestion = 0;
		previousQuestionButton.interactable = false;

		if (data.questionList.Count <= 1)
			nextQuestionButton.interactable = false;

		HideAnswer();
		if (data.questionList.Count > 0)
			UpdateQuestion();
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

		if (currentQuestion >= data.questionList.Count)
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
		showAnswerButton.gameObject.SetActive(true);
	}

	public void HideAnswer()
	{
		answerGameObject.SetActive(false);
		showAnswerButton.gameObject.SetActive(true);
	}

	#endregion

	#region Editor

	[SerializeField]
	TMP_InputField questionField;
	[SerializeField]
	TMP_InputField answerField;

	[SerializeField]
	string filePath = "/ClosestAnswer.xml";

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

	public void Save()
	{
		SaveMgr.Serialize(data, filePath);
	}

	public void Load()
	{
		DataClosestAnswer d = SaveMgr.Deserialize<DataClosestAnswer>(filePath);
		if (d != null)
		{
			data.questionList = d.questionList;
			data.answerList = d.answerList;
		}
	}

	#endregion

	private void Start()
	{
		nextQuestionButton.onClick.AddListener(NextQuestion);
		previousQuestionButton.onClick.AddListener(PreviousQuestion);
		showAnswerButton.onClick.AddListener(ShowAnswer);

		Load();
		InitGame();
	}
}

[Serializable]
[XmlRoot("DataClosestAnswer")]
public class DataClosestAnswer
{
	[XmlAttribute("questionList")]
	public List<string> questionList = new List<string>();
	[XmlAttribute("answerList")]
	public List<string> answerList = new List<string>();
}
