using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[XmlRoot("Equipes")]
public class ClosestAnswerMgr : MonoBehaviour
{
	#region Game Variables

	[SerializeField]
	TextMeshProUGUI questionText;
	[SerializeField]
	Button nextQuestionButton;
	[SerializeField]
	Button previousQuestionButton;

	[SerializeField]
	[XmlArray("questionList")]
	List<string> questionList = new List<string>();
	[XmlArray("answerList")]
	List<string> answerList = new List<string>();

	#endregion

	#region Editor Variables

	[SerializeField]
	TMP_InputField questionField;
	[SerializeField]
	TMP_InputField answerField;

	[SerializeField]
	string filePath = "ClosestAnswer";

	#endregion

	private void Start()
	{
		questionField.onSubmit.AddListener(delegate { AddQuestion(); });


	}

	public void AddQuestion()
	{
		questionList.Add(questionField.text);
		Save(filePath);
	}

	public void Save(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(ClosestAnswerMgr));
		using (FileStream stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, this);
		}
	}
}
