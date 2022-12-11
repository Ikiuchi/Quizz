using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SimpleFileBrowser.FileBrowser;

public class MyTest : MonoBehaviour
{
	[SerializeField]
	Button button = null;

	string folderPath = Application.dataPath + StaticVariables.imageToLoad;

	OnSuccess s;
	OnCancel c;

	private void Start()
	{
		button.onClick.AddListener(ClickButton);
		s += Yep;
	}
	public void ClickButton()
	{
		ShowLoadDialog(s, c, PickMode.Folders);
	}

	public void Yep(string[] str)
	{
		foreach(string str2 in str)
			Debug.Log(str2);
	}

}
