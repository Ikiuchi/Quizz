using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MysteryBoxMgr : MonoBehaviour
{
	DataQuestion data = new DataQuestion();

	List<Button> listButton = new List<Button>();

	[SerializeField]
	TextMeshProUGUI questionText;
	[SerializeField]
	Button validAnswerButton;

	[SerializeField]
	GameObject questionGameObject;
	[SerializeField]
	GameObject prefabButton;

	int currentIndex = 0;

	private void Start()
	{
		Load();
		CreateArrayButton();
		InitButton();
	}

	private void CreateArrayButton()
	{
		for (int i = 0; i < data.questionList.Count; i++)
		{
			GameObject go = Instantiate(prefabButton, transform);
			go.AddComponent<DisableButton>();
			listButton.Add(go.GetComponent<Button>());
		}
	}
	private void InitButton()
	{
		int i = 0;
		foreach (Button button in listButton)
		{
			button.onClick.AddListener(() => StartQuestion(i));
			i++;
		}

		validAnswerButton.onClick.AddListener(DisplayAnswer);
	}

	private void StartQuestion(int i)
	{
		Debug.Log(i);
		currentIndex = i;
		DisplayQuestion();
	}

	private void DisplayQuestion()
	{
		questionText.text = data.questionList[currentIndex];
		questionGameObject.SetActive(true);
	}
	private void HideQuestion()
	{
		validAnswerButton.onClick.RemoveListener(HideQuestion);
		validAnswerButton.onClick.AddListener(DisplayAnswer);
		questionGameObject.SetActive(false);
	}

	private void DisplayAnswer()
	{
		questionText.text = data.answerList[currentIndex];
		validAnswerButton.onClick.AddListener(HideQuestion);
		validAnswerButton.onClick.RemoveListener(DisplayAnswer);
	}

	public void Load()
	{
		DataQuestion d = SaveMgr.Deserialize<DataQuestion>(StaticVariables.filePathMysteryBox);
		if (d != null)
			RecoverSpace();
	}

	private void RecoverSpace()
	{
		for (int i = 0; i < data.questionList.Count; i++)
		{
			data.questionList[i] = SaveMgr.RecoverSpace(data.questionList[i]);
			data.answerList[i] = SaveMgr.RecoverSpace(data.answerList[i]);
		}
	}
}
