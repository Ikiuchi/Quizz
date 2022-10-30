using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableButton : MonoBehaviour
{
	Button button;

    void Start()
    {
		button = GetComponent<Button>();
		button.onClick.AddListener(Disable);
	}
	private void Disable()
	{
		button.interactable = false;
	}

	public void OnRevert()
	{
		button.interactable = true;
	}
}
