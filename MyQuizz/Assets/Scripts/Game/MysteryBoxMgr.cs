using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[XmlRoot("DataMysteryBox")]
public class DataGrid
{
	[XmlAttribute("gridCount")]
	public int gridSize = 0;
	[XmlAttribute("questionList")]
	public List<string> questionList = new List<string>();
	[XmlAttribute("answerList")]
	public List<string> answerList = new List<string>();
	[XmlAttribute("colorList")]
	public List<string> colorList = new List<string>();
}

public class MysteryBoxMgr : MonoBehaviour
{
	#region Game Variables

	DataGrid data = new DataGrid();

	[SerializeField]
	GameObject gridPanelGame = null;

	List<MysteryBoxQuestion> mysteryBoxes = new List<MysteryBoxQuestion>();

	#endregion

	#region Editor Variables

	[SerializeField]
	TMP_InputField gridSizeText = null;
	[SerializeField]
	TMP_InputField questionInputField = null;
	[SerializeField]
	TMP_InputField answerInputField = null;
	[SerializeField]
	GameObject prefabButton = null;
	[SerializeField]
	GameObject gridPanelEditor = null;
	[SerializeField]
	GameObject questionPanelEditor = null;

	[SerializeField]
	GameObject buttonBackToMenu = null;
	[SerializeField]
	Button buttonBackToGrid = null;
	[SerializeField]
	List<Button> colorButtonsList = new List<Button>();
	[SerializeField]
	Color neutralColor;
	[SerializeField]
	GameObject saveButton = null;

	int questionIndexSelected = 0;

	#endregion

	private void Start()
	{
		gridSizeText.keyboardType = TouchScreenKeyboardType.NumberPad;
		buttonBackToGrid.onClick.AddListener(DisplayGrid);

		gridSizeText.onSubmit.AddListener(delegate { UpdateEditorGrid(int.Parse(gridSizeText.text)); });

		questionInputField.onSubmit.AddListener(delegate { SetQuestionText(); });
		questionInputField.onEndEdit.AddListener(delegate { SetQuestionText(); });

		answerInputField.onSubmit.AddListener(delegate { SetAnswerText(); });
		answerInputField.onEndEdit.AddListener(delegate { SetAnswerText(); });

		foreach (Button button in colorButtonsList)
		{
			button.onClick.AddListener(delegate { SetColor(button.colors.disabledColor); });
		}

		InitGame();
		InitEditor();
	}

	#region Game Methods

	public void InitGame()
	{
		Load();
		UpdateGameGrid();
	}

	private void UpdateGameGrid()
	{
		foreach (Transform child in gridPanelGame.transform)
			Destroy(child.gameObject);

		mysteryBoxes.Clear();

		for (int i = 0; i < data.gridSize; i++)
		{
			int x = i;
			GameObject g = Instantiate(prefabButton, gridPanelGame.transform);
			g.GetComponent<Button>().onClick.AddListener(() => OnQuestionClick(x));

			InitMysteryBoxQuestion(g, i);
		}
	}

	private void OnQuestionClick(int index)
	{
		gridPanelGame.SetActive(false);
		mysteryBoxes[index].OnClick();
	}

	private void InitMysteryBoxQuestion(GameObject g, int index)
	{
		MysteryBoxQuestion question = g.GetComponent<MysteryBoxQuestion>();
		InitQuestion(question, index);
	}

	private void InitQuestion(MysteryBoxQuestion question, int index)
	{
		question.questionText = data.questionList[index];
		question.answerText = data.answerList[index];
		question.buttonText.text = (index + 1).ToString();

		Color color = neutralColor;
		if (ColorUtility.TryParseHtmlString(data.colorList[index], out color))
			question.SetActiveColor(color);
		mysteryBoxes.Add(question);
	}

	#endregion

	#region Editor Methods

	public void InitEditor()
	{
		DisplayGrid();
		UpdateEditorGrid(data.gridSize);
	}

	private void UpdateData(int newSize)
	{
		if (data.gridSize == newSize)
			return;

		if (data.gridSize == 0)
		{
			for (int i = 0; i < newSize; i++)
			{
				data.questionList.Add("");
				data.answerList.Add("");
				data.colorList.Add("#"+ColorUtility.ToHtmlStringRGBA(neutralColor));
			}
		}
		else
		{
			int difference = Mathf.Abs(newSize - data.gridSize);

			if (data.gridSize < newSize)
			{
				//Add new entries
				for (int i = 0; i < difference; i++)
				{
					data.questionList.Add("");
					data.answerList.Add("");
					data.colorList.Add(ColorUtility.ToHtmlStringRGBA(neutralColor));
				}
			}
			else
			{
				//Remove last entries
				data.questionList.RemoveRange(newSize, difference);
				data.answerList.RemoveRange(newSize, difference);
				data.colorList.RemoveRange(newSize, difference);
			}
		}

		data.gridSize = newSize;
	}

	private void UpdateEditorGrid(int gridSize)
	{
		UpdateData(gridSize);
		CreateEditorGrid(gridSize);
		Save();
	}

	private void CreateEditorGrid(int gridSize)
	{
		foreach (Transform child in gridPanelEditor.transform)
			Destroy(child.gameObject);

		mysteryBoxes.Clear();

		for (int i = 0; i < gridSize; i++)
		{
			int x = i;
			GameObject g = Instantiate(prefabButton, gridPanelEditor.transform);
			g.GetComponent<Button>().onClick.AddListener(() => DisplayQuestionPanel(x));

			InitMysteryBoxQuestion(g, i);
		}
	}

	private void SetQuestionText()
	{
		mysteryBoxes[questionIndexSelected].questionText = questionInputField.text;
		data.questionList[questionIndexSelected] = questionInputField.text;
	}

	private void SetAnswerText()
	{
		mysteryBoxes[questionIndexSelected].answerText = answerInputField.text;
		data.answerList[questionIndexSelected] = answerInputField.text;
	}

	public void DisplayGrid()
	{
		questionPanelEditor.SetActive(false);
		gridPanelEditor.SetActive(true);
		buttonBackToMenu.SetActive(true);
		gridSizeText.gameObject.SetActive(true);
		saveButton.SetActive(true);
	}

	private void DisplayQuestionPanel(int index)
	{
		buttonBackToMenu.SetActive(false);
		gridPanelEditor.SetActive(false);
		gridSizeText.gameObject.SetActive(false);
		questionPanelEditor.SetActive(true);
		saveButton.SetActive(false);

		questionIndexSelected = index;

		questionInputField.text = mysteryBoxes[questionIndexSelected].questionText;
		answerInputField.text = mysteryBoxes[questionIndexSelected].answerText;
	}

	private void SetColor(Color color)
	{
		mysteryBoxes[questionIndexSelected].SetActiveColor(color);
		data.colorList[questionIndexSelected] = "#" + ColorUtility.ToHtmlStringRGBA(color);
	}

	#endregion


	#region SaveAndLoad

	private void RemoveSpace()
	{
		for (int i = 0; i < data.gridSize; i++)
		{
			data.questionList[i] = SaveMgr.RemoveSpace(data.questionList[i]);
			data.answerList[i] = SaveMgr.RemoveSpace(data.answerList[i]);
		}
	}

	private void RecoverSpace()
	{
		for (int i = 0; i < data.gridSize; i++)
		{
			data.questionList[i] = SaveMgr.RecoverSpace(data.questionList[i]);
			data.answerList[i] = SaveMgr.RecoverSpace(data.answerList[i]);
		}
	}

	public void Save()
	{
		RemoveSpace();
		SaveMgr.Serialize(data, StaticVariables.filePathMysteryBox);
	}

	public void Load()
	{
		DataGrid _data = SaveMgr.Deserialize<DataGrid>(StaticVariables.filePathMysteryBox);
		if (_data != null)
		{
			data = _data;
			RecoverSpace();
		}
	}

	#endregion
}
