using TMPro;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
	[SerializeField]
	TMP_InputField numberOfCaseEdit;

	int numberOfCase = 0;
	[SerializeField]
	GameObject prefabButton;

	private void Start()
	{
		numberOfCaseEdit.contentType = TMP_InputField.ContentType.IntegerNumber;
		numberOfCaseEdit.onValueChanged.AddListener(OnChooseNumber);
	}

	private void OnChooseNumber(string value)
	{
		numberOfCase = int.Parse(value);
		CreateArrayButton();
	}

	private void CreateArrayButton()
	{
		foreach (Transform child in transform)
			Destroy(child.gameObject);

		for (int i = 0; i < numberOfCase; i++)
		{
			GameObject go = Instantiate(prefabButton, transform);
			go.AddComponent<DisableButton>();
		}
	}

	
}
