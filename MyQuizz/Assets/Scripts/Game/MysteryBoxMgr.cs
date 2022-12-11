using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[XmlRoot("DataClosestAnswer")]
public class DataGrid
{
	[XmlAttribute("gridCount")]
	public int gridSize = 0;
}

public class MysteryBoxMgr : MonoBehaviour
{
	#region Game Variables

	DataGrid data = new DataGrid();

	[SerializeField]
	GameObject gridPanelGame = null;

	#endregion

	#region Editor Variables

	[SerializeField]
	TMP_InputField gridSizeText = null;
	[SerializeField]
	GameObject prefabButton = null;
	[SerializeField]
	GameObject gridPanelEditor = null;

	#endregion

	private void Start()
	{
		gridSizeText.keyboardType = TouchScreenKeyboardType.NumberPad;
		gridSizeText.onSubmit.AddListener(delegate { UpdateEditorGrid(); });

		//UpdateGameGrid();
	}

	#region Game Methods

	private void UpdateGameGrid()
	{
		foreach (Transform child in gridPanelGame.transform)
			Destroy(child.gameObject);

		for (int i = 0; i < data.gridSize; i++)
		{
			GameObject g = Instantiate(prefabButton, gridPanelGame.transform);
		}
	}

	#endregion

	#region Editor Methods

	private void UpdateEditorGrid()
	{
		foreach (Transform child in gridPanelEditor.transform)
			Destroy(child.gameObject);

		int gridSize = int.Parse(gridSizeText.text);
		data.gridSize = gridSize;

		for (int i = 0; i < gridSize; i ++)
		{
			GameObject g = Instantiate(prefabButton, gridPanelEditor.transform);
		}

		Save();
	}

	#endregion


	#region SaveAndLoad

	public void Save()
	{
		SaveMgr.Serialize(data, StaticVariables.filePathMysteryBox);
	}

	public void Load()
	{
		DataGrid _data = SaveMgr.Deserialize<DataGrid>(StaticVariables.filePathMysteryBox);
		if (_data != null)
		{
			data = _data;
		}
	}

	#endregion
}
