using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum EQuestionState
{
	Neutral,
	Active
}

public class MysteryBoxQuestion : MonoBehaviour
{
	#region Variables

	[SerializeField]
	Button button = null;
	public TextMeshProUGUI buttonText = null;

	[HideInInspector]
	public string questionText = "";
	[HideInInspector]
	public string answerText = "";

	[SerializeField]
	Color neutralColor;
	Color activeColor;

	EQuestionState state = EQuestionState.Neutral;

	#endregion

	public void SetActiveColor(Color color)
	{
		activeColor = color;
		SetNormalColor(color);
	}

	private void SetNormalColor(Color color)
	{
		ColorBlock block = button.colors;
		block.normalColor = color;
		button.colors = block;
	}

	public Color GetColor()
	{
		if (state == EQuestionState.Active)
			return activeColor;

		return neutralColor;
	}

	public void OnClick()
	{
		if (state != EQuestionState.Active)
			ActiveState();
		else
			NeutralState();
	}

	private void ActiveState()
	{
		state = EQuestionState.Active;
		SetNormalColor(activeColor);
	}

	private void NeutralState()
	{
		state = EQuestionState.Neutral;
		SetNormalColor(neutralColor);
	}
}
