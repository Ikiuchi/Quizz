using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

using static SimpleFileBrowser.FileBrowser;

[Serializable]
[XmlRoot("DataAgainstTimeTitle")]
public class DataAgainstTimeTitle
{
	[XmlAttribute("titleList")]
	public List<string> titleList = new List<string>();
}

public class AgainstTimeMgr : MonoBehaviour
{
	string folderPath = Application.dataPath + StaticVariables.imageToLoad + "Folder";

	#region Game variables

	[SerializeField]
	List<Button> categoryButtonList = new List<Button>();
	[SerializeField]
	List<TMP_Text> titleList = new List<TMP_Text>();

	List<Texture2D> textureList = new List<Texture2D>();
	[SerializeField]
	Texture defaultTextureNotReady;
	[SerializeField]
	Texture defaultTextureReady;
	Vector2 ImageSize;

	[SerializeField]
	GameObject categoriesPanel = null;
	[SerializeField]
	GameObject categoryPanel = null;

	int currentIndexTexture = 0;

	[SerializeField]
	Button nextImageButton = null;
	[SerializeField]
	Button previousImageButton = null;
	[SerializeField]
	Button backToCategoriesButton = null;

	[SerializeField]
	RawImage rawImage;
	#endregion

	#region Editor variables

	DataAgainstTimeTitle data = new DataAgainstTimeTitle();
	[SerializeField]
	List<TMP_InputField> inputFieldsTitleList = new List<TMP_InputField>();
	[SerializeField]
	List<Button> categoryEditButtonList = new List<Button>();
	OnSuccess onSuccessOpenFolder;

	int indexCache;

	#endregion

	private void Start()
	{
		if (inputFieldsTitleList.Count != titleList.Count)
			Debug.LogError("Not the same count between editor and game");

		var rectTransform = rawImage.GetComponent<RectTransform>();
		if (rectTransform != null)
			ImageSize = rectTransform.sizeDelta;

		onSuccessOpenFolder += SuccessOpenFolder;

		Load();
		InitEditor();
		InitGame();
		InitGameButtons();
	}

	#region Game

	public void ReloadGame()
	{
		Load();
		InitGame();
	}

	public void InitGame()
	{
		categoriesPanel.SetActive(true);
		categoryPanel.SetActive(false);

		for (int i = 0; i < data.titleList.Count; i++)
			titleList[i].text = data.titleList[i];
	}

	private void InitCategory(int index)
	{
		InitCategoryPanel();

		textureList.Clear();

		string path;
		path = folderPath + index;
		if (!Directory.Exists(path))
			return;

		StartCoroutine(LoadTexture(path));
	}

	private void InitGameButtons()
	{
		nextImageButton.onClick.AddListener(NextImage);
		previousImageButton.onClick.AddListener(PreviousImage);
		backToCategoriesButton.onClick.AddListener(BackToCategories);

		for (int i = 0; i < categoryButtonList.Count; i++)
		{
			int x = i;
			categoryButtonList[i].onClick.AddListener(() => InitCategory(x));
		}
	}

	private void InitCategoryPanel()
	{
		currentIndexTexture = -1;
		rawImage.texture = defaultTextureNotReady;
		previousImageButton.interactable = false;
		nextImageButton.interactable = false;

		categoriesPanel.SetActive(false);
		categoryPanel.SetActive(true);
	}

	public void StartCategory()
	{
		rawImage.texture = defaultTextureReady;
		nextImageButton.interactable = true;
	}

	public void NextImage()
	{
		LoadImage.KeepGameObjectSize(rawImage, ImageSize);
		rawImage.texture = textureList[++currentIndexTexture];
		rawImage.SizeToParent();

		if (currentIndexTexture >= textureList.Count - 1)
			nextImageButton.interactable = false;
		if (currentIndexTexture > 0) //The first iteration put currentIndexTexture to 0
			previousImageButton.interactable = true;
	}

	public void PreviousImage()
	{
		LoadImage.KeepGameObjectSize(rawImage, ImageSize);
		rawImage.texture = textureList[--currentIndexTexture];
		rawImage.SizeToParent();

		if (currentIndexTexture == 0)
			previousImageButton.interactable = false;
		
		nextImageButton.interactable = true;
	}

	public void BackToCategories()
	{
		categoriesPanel.SetActive(true);
		categoryPanel.SetActive(false);
	}

	private IEnumerator LoadTexture(string path)
	{
		FileInfo[] fileInfo = LoadImage.LoadFilesFromFolder(path);
		foreach (var file in fileInfo)
		{
			if (file.Name.EndsWith(".meta"))
				continue;

			WWW www = new WWW(path + "/" + file.Name);
			while (!www.isDone)
				yield return null;

			textureList.Add(www.texture.makeTextureReadable());
		}

		StartCategory();
	}

	#endregion

	#region Editor

	private void InitEditor()
	{
		for (int i = 0; i < categoryEditButtonList.Count; i++)
		{
			int x = i;
			categoryEditButtonList[i].onClick.AddListener(() => OpenFolderWindow(x));
		}

		for(int i = 0; i < data.titleList.Count; i++)
			inputFieldsTitleList[i].text = data.titleList[i];
	}

	private void OpenFolderWindow(int index)
	{
		indexCache = index;
		ShowLoadDialog(onSuccessOpenFolder, null, PickMode.Folders);
	}

	public void SuccessOpenFolder(string[] path)
	{
		textureList.Clear();
		StartCoroutine(EditCategory(path[0]));
	}

	public IEnumerator EditCategory(string path)
	{
		var fileInfo = LoadImage.LoadFilesFromFolder(path);

		foreach (var file in fileInfo)
		{
			if (file.Name.EndsWith(".meta"))
				continue;

			WWW www = new WWW(path + "/" + file.Name);
			while (!www.isDone)
				yield return null;

			SaveTexture(indexCache, www.texture, file.Name);
		}
	}

	private void SaveTexture(int indexFolder, Texture2D texture, string imageName)
	{
		// Encode texture into PNG
		byte[] bytes = texture.makeTextureReadable().EncodeToPNG();
		string path = CreateFolder(indexFolder) + "/";
		System.IO.File.WriteAllBytes(path + imageName, bytes);
#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh();
#endif
	}

	private string CreateFolder(int index)
	{
		string path = folderPath + index;
		DirectoryInfo folder;
		if (!Directory.Exists(path))
			folder = Directory.CreateDirectory(path);
	
		return path;
	}

	public void SaveCategoryTitles()
	{
		data.titleList.Clear();
		for (int i = 0; i < inputFieldsTitleList.Count; i++)
			data.titleList.Add(inputFieldsTitleList[i].text);
		
		Save();
	}

	#endregion

	#region SaveAndLoad
	private void RemoveSpace()
	{
		for (int i = 0; i < data.titleList.Count; i++)
			data.titleList[i] = SaveMgr.RemoveSpace(data.titleList[i]);
	}

	private void RecoverSpace()
	{
		for (int i = 0; i < data.titleList.Count; i++)
			data.titleList[i] = SaveMgr.RecoverSpace(data.titleList[i]);
	}

	public void Save()
	{
		RemoveSpace();
		SaveMgr.Serialize(data, StaticVariables.filePathAgainstTime);
	}

	public void Load()
	{
		DataAgainstTimeTitle _data = SaveMgr.Deserialize<DataAgainstTimeTitle>(StaticVariables.filePathAgainstTime);
		if (_data != null)
		{
			data = _data;
			RecoverSpace();
		}
	}

	#endregion
}
