using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeselectButton : MonoBehaviour
{
	Button button;
	EventSystem eventSystem;

	void Start()
    {
        button = GetComponent<Button>();
		button.onClick.AddListener(Deselect);

		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}

	void Deselect()
	{
		eventSystem.SetSelectedGameObject(null);
	}

}
