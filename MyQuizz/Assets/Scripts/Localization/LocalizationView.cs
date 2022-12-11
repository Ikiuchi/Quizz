using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationView : MonoBehaviour
{
	public Color inactivColor;
	public List<Image> flags = new List<Image>();

	private void Start()
	{
		StartCoroutine(StartInit());
	}

	public void OnSelectLangage(Image _image)
	{
		foreach (Image flag in flags)
		{
			if (_image != flag)
				flag.color = inactivColor;
		}

		_image.color = Color.white;
	}

	private IEnumerator StartInit()
	{
		while(!LocalizationManager.Instance.Initialized)
			yield return null;

		InitViewFlags(LocalizationManager.Instance.GetId);
	}

	private void InitViewFlags(int flagId)
	{
		for (int i = 0; i < flags.Count; i++)
		{
			if (flagId == i)
				flags[i].color = Color.white;
			else
				flags[i].color = inactivColor;
		}
	}
}
